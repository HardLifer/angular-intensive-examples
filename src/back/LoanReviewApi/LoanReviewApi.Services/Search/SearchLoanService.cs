using LoanReviewApi.Core.Enums;
using LoanReviewApi.Infrastructure.Context;
using LoanReviewApi.Infrastructure.QueryFilters;
using LoanReviewApi.Services.DTOs;
using LoanReviewApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace LoanReviewApi.Services.Search
{
    public class SearchLoanService : ISearchLoanService
    {
        private readonly ILogger<SearchLoanService> _logger;
        private readonly IDbContextFactory<LoanReviewContext> _dbContextFactory;
        private readonly HybridCache _hybridCache;

        public SearchLoanService(ILogger<SearchLoanService> logger, IDbContextFactory<LoanReviewContext> dbContextFactory, HybridCache hybridCache)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
            _hybridCache = hybridCache;
        }

        public async Task<LoanReviewDetailDto> GetLoanDetailById(int loanId, CancellationToken cancellationToken)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

            var loanDetailDto = await dbContext.LoanDetails
                .Select(ent => new LoanReviewDetailDto
                {
                    Id = ent.Id,
                    LoanId = ent.LoanId
                })
                .FirstOrDefaultAsync(ent => ent.LoanId == loanId);

            if (loanDetailDto == null)
            {
                // Log Error
            }

            return loanDetailDto!;
        }

        public async Task<SearchResponseDto> Search(SearchRequestDto searchRequest, CancellationToken cancellationToken)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

            var generalQuery = dbContext.LoanReviewDetails.AsQueryable();

            if (searchRequest.TemplateId == (int)ReviewTemplates.Residential)
            {
                generalQuery = generalQuery.Include(ent => ent.LoanDetail);
            }
            else
            {
                generalQuery = generalQuery.Include(ent => ent.LoanDetailsComm);
            }

            if (!string.IsNullOrWhiteSpace(searchRequest.Filters.Director))
            {
                generalQuery = generalQuery.SearchByDirector(searchRequest.TemplateId, searchRequest.Filters.Director);
            }

            if (!string.IsNullOrWhiteSpace(searchRequest.Filters.Team))
            {
                generalQuery = generalQuery.SearchByTeam(searchRequest.TemplateId, searchRequest.Filters.Team);
            }

            if (!string.IsNullOrWhiteSpace(searchRequest.Filters.ProductCode))
            {
                generalQuery = generalQuery.SearchByProductCode(searchRequest.TemplateId, searchRequest.Filters.ProductCode);
            }

            if (!string.IsNullOrWhiteSpace(searchRequest.Filters.Reviewer))
            {
                generalQuery = generalQuery.SearchByReviewer(int.Parse(searchRequest.Filters.Reviewer));
            }

            if (!string.IsNullOrWhiteSpace(searchRequest.Filters.ReviewType))
            {
                generalQuery = generalQuery.SearchByReviewType(searchRequest.TemplateId, searchRequest.Filters.ReviewType);
            }

            if (searchRequest.Filters.Status != null)
            {
                generalQuery = generalQuery.SearchByStatus((int?)searchRequest.Filters.Status);
            }

            if (searchRequest.Filters.Month != null)
            {
                generalQuery = generalQuery.SearchByMonth(searchRequest.TemplateId, searchRequest.Filters.Month);
            }

            if (searchRequest.Filters.QuarterStart != null && searchRequest.Filters.QuarterEnd != null)
            {
                generalQuery = generalQuery.SearchByQuarter(searchRequest.TemplateId, searchRequest.Filters.QuarterStart, searchRequest.Filters.QuarterEnd);
            }

            // Pagination metadata
            var totalItems = await generalQuery.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalItems / (double)searchRequest.Pagination.PageSize);

            var loans = await generalQuery
                .AsNoTracking()
                .AsSplitQuery()
                .Include(ent => ent.UserCompletedBy)
                .OrderBy(ent => ent.LoanId)
                .Skip((searchRequest.Pagination.PageNumber - 1) * searchRequest.Pagination.PageSize)
                .Take(searchRequest.Pagination.PageSize)
                .Select(ent => new LoanReviewDetailDto
                {
                    LoanId = ent.LoanId,
                    Status = (LoanReviewStatuses)ent.StatusId!,
                    Id = ent.Id,
                    Branch = searchRequest.TemplateId == (int)ReviewTemplates.Residential 
                        ? ent.LoanDetail!.BranchOfficeId : ent.LoanDetailsComm!.BranchOfficeId,
                    Province = searchRequest.TemplateId == (int)ReviewTemplates.Residential
                        ? ent.LoanDetail!.Province : ent.LoanDetailsComm!.Province,
                    Director = searchRequest.TemplateId == (int)ReviewTemplates.Residential
                        ? ent.LoanDetail!.Director : ent.LoanDetailsComm!.Director,
                    BorrowerName = searchRequest.TemplateId == (int)ReviewTemplates.Residential
                        ? ent.LoanDetail!.BorrowerName : ent.LoanDetailsComm!.BorrowerName,
                    ProductCode = searchRequest.TemplateId == (int)ReviewTemplates.Residential
                        ? ent.LoanDetail!.ProductBusinessGroupCode : ent.LoanDetailsComm!.ProductBusinessGroupCode,
                    ReviewType = searchRequest.TemplateId == (int)ReviewTemplates.Residential
                        ? ent.LoanDetail!.ReviewType : ent.LoanDetailsComm!.ReviewType,
                    Date = searchRequest.TemplateId == (int)ReviewTemplates.Residential
                        ? ent.LoanDetail!.Date1 : ent.LoanDetailsComm!.Date1,
                    MortgageOfficer = searchRequest.TemplateId == (int)ReviewTemplates.Residential
                        ? ent.LoanDetail!.MortgageOfficerName : ent.LoanDetailsComm!.MortgageOfficerName,
                    ReviewedBy = ent.UserCompletedBy!.UserName
                })
                .ToListAsync(cancellationToken);

            var pageInfo = new TotalItemsDto(
                TotalPages: totalPages,
                TotalItems: totalItems,
                CurrentPage: searchRequest.Pagination.PageNumber,
                Size: searchRequest.Pagination.PageSize
            );

            return new SearchResponseDto(loans, pageInfo);
        }

        public async Task<IDictionary<string, List<string>>> GetSearchFilters(int templateId, CancellationToken cancellationToken)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            Dictionary<string, List<string>> filters = new Dictionary<string, List<string>>(8);

            try
            {
                var generalQuery = dbContext.LoanReviewDetails.AsQueryable();

                if (templateId == (int)ReviewTemplates.Residential)
                {
                    generalQuery = generalQuery.Include(ent => ent.LoanDetail).Where(ent => ent.LoanDetail != null);

                    var monthQuery = generalQuery;

                    // Month
                    filters.Add("month-filter", new List<string>() { "All" });
                    var earliestAddedResedentialLoan = await monthQuery.AsNoTracking().MinAsync(ent => ent.LoanDetail!.Date1, cancellationToken);
                    var latestAddedResedentialLoan = await monthQuery.AsNoTracking().MaxAsync(ent => ent.LoanDetail!.Date1, cancellationToken);

                    var monthFilterValues = GetMonthFilterValue(earliestAddedResedentialLoan, latestAddedResedentialLoan);

                    if (monthFilterValues.Any())
                    {
                        filters["month-filter"].AddRange(monthFilterValues);
                    }

                    // Quarters
                    filters.Add("querters-filter", new List<string>() { "All" });
                    var quertersFilterValue = GetQuartersFilterValue(earliestAddedResedentialLoan, latestAddedResedentialLoan);

                    if (quertersFilterValue.Any())
                    {
                        filters["querters-filter"].AddRange(quertersFilterValue);
                    }

                    // Statuses
                    filters.Add("status-filter", new List<string>() { "All" });

                    var statuses = await dbContext.LoanReviewStatus
                        .AsNoTracking()
                        .OrderByDescending(ent => ent.Id)
                        .Select(ent => ent.Status)
                        .ToListAsync(cancellationToken);

                    if (statuses.Any())
                    {
                        filters["status-filter"].AddRange(statuses);
                    }

                    // Products
                    var productQuery = generalQuery;
                    filters.Add("products-filter", new List<string>() { "All" });

                    var productValues = await productQuery
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(ent => ent.LoanDetail!.ProductBusinessGroupCode != null)
                        .Select(ent => ent.LoanDetail!.ProductBusinessGroupCode)
                        .Distinct()
                        .OrderBy(ent => ent)
                        .ToListAsync(cancellationToken);

                    if (productValues.Any())
                    {
                        filters["products-filter"].AddRange(productValues!);
                    }

                    // Review Types
                    var reviewTypeQuery = generalQuery;
                    filters.Add("reviewtypes-filter", new List<string>() { "All" });

                    var reviewTypesValues = await reviewTypeQuery
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(ent => ent.LoanDetail!.ReviewType != null)
                        .Select(ent => ent.LoanDetail!.ReviewType)
                        .Distinct()
                        .OrderBy(ent => ent)
                        .ToListAsync(cancellationToken);

                    if (reviewTypesValues.Any())
                    {
                        var randomValueIndex = reviewTypesValues.IndexOf("RANDOM");
                        if (randomValueIndex != -1)
                        {
                            reviewTypesValues.RemoveAt(randomValueIndex);
                            reviewTypesValues.Insert(0, "RANDOM");
                        }

                        filters["reviewtypes-filter"].AddRange(productValues!);
                    }

                    // branch/teams
                    var teamsQuery = generalQuery;
                    filters.Add("teams-filter", new List<string>() { "All" });

                    var branchTeamValues = await teamsQuery
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(ent => ent.LoanDetail!.BranchOfficeId != null
                            && !string.IsNullOrWhiteSpace(ent.LoanDetail!.BranchOfficeId)
                            && !ent.LoanDetail!.BranchOfficeId.StartsWith("0"))
                        .Select(ent => new { text = ent.LoanDetail!.BranchOfficeText, value = ent.LoanDetail!.BranchOfficeId })
                        .Distinct()
                        .OrderBy(ent => ent.value)
                        .ToListAsync(cancellationToken);

                    if (branchTeamValues.Any())
                    {
                        filters["teams-filter"].AddRange(branchTeamValues.Select(ent => string.Concat(ent.text, '|', ent.value)));
                    }

                    // Directors
                    var directorsQuery = generalQuery;
                    filters.Add("directors-filetrs", new List<string>() { "All" });

                    var directorsValues = await directorsQuery
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(ent => ent.LoanDetail!.Director != null && !string.IsNullOrWhiteSpace(ent.LoanDetail!.Director))
                        .Select(ent => ent.LoanDetail!.Director)
                        .Distinct()
                        .OrderBy(ent => ent)
                        .ToListAsync(cancellationToken);

                    if (directorsValues.Any())
                    {
                        filters["directors-filetrs"].AddRange(directorsValues!);
                    }

                    // Reviewers
                    var reviewersQuery = generalQuery;
                    filters.Add("reviewers-filetrs", new List<string>() { "All" });

                    var reviewersValues = await reviewersQuery
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Include(ent => ent.UserCompletedBy)
                        .Where(ent => ent.UserCompletedBy != null)
                        .Where(ent => ent.CompletedBy != null)
                        .Select(ent => ent.UserCompletedBy!.UserName)
                        .Distinct()
                        .OrderBy(ent => ent)
                        .ToListAsync(cancellationToken);

                    if (reviewersValues.Any())
                    {
                        filters["reviewers-filetrs"].AddRange(reviewersValues!);
                    }

                    return filters;
                }
                else
                {
                    generalQuery = generalQuery.Include(ent => ent.LoanDetailsComm).Where(ent => ent.LoanDetailsComm != null);

                    var monthQuery = generalQuery;

                    // Month
                    filters.Add("month-filter", new List<string>() { "All" });
                    var earliestAddedResedentialLoan = await monthQuery.AsNoTracking().MinAsync(ent => ent.LoanDetailsComm!.Date1, cancellationToken);
                    var latestAddedResedentialLoan = await monthQuery.AsNoTracking().MaxAsync(ent => ent.LoanDetailsComm!.Date1, cancellationToken);

                    var monthFilterValues = GetMonthFilterValue(earliestAddedResedentialLoan, latestAddedResedentialLoan);

                    if (monthFilterValues.Any())
                    {
                        filters["month-filter"].AddRange(monthFilterValues);
                    }

                    // Quarters
                    filters.Add("querters-filter", new List<string>() { "All" });
                    var quertersFilterValue = GetQuartersFilterValue(earliestAddedResedentialLoan, latestAddedResedentialLoan);

                    if (quertersFilterValue.Any())
                    {
                        filters["querters-filter"].AddRange(quertersFilterValue);
                    }

                    // Statuses
                    filters.Add("status-filter", new List<string>() { "All" });

                    var statuses = await dbContext.LoanReviewStatus
                        .AsNoTracking()
                        .OrderByDescending(ent => ent.Id)
                        .Select(ent => ent.Status)
                        .ToListAsync(cancellationToken);

                    if (statuses.Any())
                    {
                        filters["status-filter"].AddRange(statuses);
                    }

                    // Products
                    var productQuery = generalQuery;
                    filters.Add("products-filter", new List<string>() { "All" });

                    var productValues = await productQuery
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(ent => ent.LoanDetailsComm!.ProductBusinessGroupCode != null)
                        .Select(ent => ent.LoanDetailsComm!.ProductBusinessGroupCode)
                        .Distinct()
                        .OrderBy(ent => ent)
                        .ToListAsync(cancellationToken);

                    if (productValues.Any())
                    {
                        filters["products-filter"].AddRange(productValues!);
                    }

                    // Review Types
                    var reviewTypeQuery = generalQuery;
                    filters.Add("reviewtypes-filter", new List<string>() { "All" });

                    var reviewTypesValues = await reviewTypeQuery
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(ent => ent.LoanDetailsComm!.ReviewType != null)
                        .Select(ent => ent.LoanDetailsComm!.ReviewType)
                        .Distinct()
                        .OrderBy(ent => ent)
                        .ToListAsync(cancellationToken);

                    if (reviewTypesValues.Any())
                    {
                        var randomValueIndex = reviewTypesValues.IndexOf("RANDOM");
                        if (randomValueIndex != -1)
                        {
                            reviewTypesValues.RemoveAt(randomValueIndex);
                            reviewTypesValues.Insert(0, "RANDOM");
                        }

                        filters["reviewtypes-filter"].AddRange(productValues!);
                    }

                    // branch/teams
                    var teamsQuery = generalQuery;
                    filters.Add("teams-filter", new List<string>() { "All" });

                    var branchTeamValues = await teamsQuery
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(ent => ent.LoanDetailsComm!.BranchOfficeId != null
                            && !string.IsNullOrWhiteSpace(ent.LoanDetailsComm!.BranchOfficeId)
                            && !ent.LoanDetailsComm!.BranchOfficeId.StartsWith("0"))
                        .Select(ent => new { text = ent.LoanDetailsComm!.BranchOfficeText, value = ent.LoanDetailsComm!.BranchOfficeId })
                        .Distinct()
                        .OrderBy(ent => ent.value)
                        .ToListAsync(cancellationToken);

                    if (branchTeamValues.Any())
                    {
                        filters["teams-filter"].AddRange(branchTeamValues.Select(ent => string.Concat(ent.text, '|', ent.value)));
                    }

                    // Directors
                    var directorsQuery = generalQuery;
                    filters.Add("directors-filetrs", new List<string>() { "All" });

                    var directorsValues = await directorsQuery
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(ent => ent.LoanDetailsComm!.Director != null && !string.IsNullOrWhiteSpace(ent.LoanDetailsComm!.Director))
                        .Select(ent => ent.LoanDetailsComm!.Director)
                        .Distinct()
                        .OrderBy(ent => ent)
                        .ToListAsync(cancellationToken);

                    if (directorsValues.Any())
                    {
                        filters["directors-filetrs"].AddRange(directorsValues!);
                    }

                    // Reviewers
                    var reviewersQuery = generalQuery;
                    filters.Add("reviewers-filetrs", new List<string>() { "All" });

                    var reviewersValues = await reviewersQuery
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Include(ent => ent.UserCompletedBy)
                        .Where(ent => ent.UserCompletedBy != null)
                        .Where(ent => ent.CompletedBy != null)
                        .Select(ent => ent.UserCompletedBy!.UserName)
                        .Distinct()
                        .OrderBy(ent => ent)
                        .ToListAsync(cancellationToken);

                    if (reviewersValues.Any())
                    {
                        filters["reviewers-filetrs"].AddRange(reviewersValues!);
                    }

                    return filters;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private IEnumerable<string> GetMonthFilterValue(DateTime? minFilterDate, DateTime? maxFilterDate)
        {
            if (!minFilterDate.HasValue && !maxFilterDate.HasValue)
            {
                return Enumerable.Empty<string>();
            }

            List<string> monthFilterValue = new List<string>();
            // Descending Order
            for (int year = maxFilterDate!.Value.Year; year >= minFilterDate!.Value.Year; year--)
            {
                int minFilterMonth = (year == minFilterDate.Value.Year) ? minFilterDate.Value.Month : 1;

                int maxFilterMonth = (year == maxFilterDate.Value.Year) ? maxFilterDate.Value.Month : 12;

                do
                {
                    string filterText = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(maxFilterMonth) + " " + year.ToString();
                    string filterValue = year.ToString() + '-' + maxFilterMonth.ToString();


                    monthFilterValue.Add(string.Concat(filterText, '|', filterValue));

                    maxFilterMonth = maxFilterMonth - 1;
                }
                while (minFilterMonth <= maxFilterMonth);
            }

            return monthFilterValue;
        }

        private IEnumerable<string> GetQuartersFilterValue(DateTime? minFilterDate, DateTime? maxFilterDate)
        {
            if (!minFilterDate.HasValue && !maxFilterDate.HasValue)
            {
                return Enumerable.Empty<string>();
            }

            List<string> quarterValues = new List<string>();
            // Descending Order
            for (int year = maxFilterDate!.Value.Year; year >= minFilterDate!.Value.Year; year--)
            {
                int maxFilterMonth = (year == maxFilterDate.Value.Year) ? maxFilterDate.Value.Month : 12;

                if ((maxFilterMonth % 3) == 0)
                {
                    var monthquarter = GetQuarter(maxFilterMonth);

                    string filterText = 'Q' + monthquarter.ToString() + '-' + year.ToString();
                    string filterValue = monthquarter.ToString() + '-' + year.ToString();


                    quarterValues.Add(string.Concat(filterText, '|', filterValue));
                }
            }

            return quarterValues;
        }

        private int GetQuarter(int month)
        {
            return month switch
            {
                >= 4 and <= 6 => 2,
                >= 7 and <= 9 => 3,
                >= 10 and <= 12 => 4,
                _ => 1
            };
        }
    }
}
