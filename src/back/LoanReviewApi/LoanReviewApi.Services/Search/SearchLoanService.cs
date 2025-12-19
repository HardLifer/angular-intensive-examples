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

		public async Task<LoanReviewDetailDto> GetLoanDetailById(int loanId, int templateId, CancellationToken cancellationToken)
		{
			_logger.LogDebug("GetLoanDetailById service methhod called with TemplateId: {TemplateId} and LoanId: {LoanId}", templateId, loanId);

			string loanCacheKey = $"GetLoanDetailById-{loanId}-{templateId}";

			var loanReview = await _hybridCache.GetOrCreateAsync(loanCacheKey, async factory =>
			{
				factory.ThrowIfCancellationRequested();

				return await ValueTask.FromResult<LoanReviewDetailDto>(new LoanReviewDetailDto());
			},
			cancellationToken: cancellationToken);

			if (loanReview.Id == 0)
			{
				_logger.LogDebug("Cached loan value with key = {key} haven't added yet. Cache entry generaterd.", loanCacheKey);
			}
			else
			{
				return loanReview;
			}

			using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

			var loanReviewRequest = dbContext.LoanReviewDetails.AsQueryable();

			switch (templateId)
			{
				case (int)ReviewTemplates.Residential:
					loanReviewRequest = loanReviewRequest
							.Include(ent => ent.LoanDetail)
							.Where(ent => ent.LoanDetail != null && ent.LoanDetail.IsLocked != 1)
							.Where(ent => ent.TemplateId == templateId);
					break;
				case (int)ReviewTemplates.Commercial:
					loanReviewRequest = loanReviewRequest
							.Include(ent => ent.LoanDetailsComm)
							.Where(ent => ent.LoanDetailsComm != null && ent.LoanDetailsComm.IsLocked != 1)
							.Where(ent => ent.TemplateId == templateId);
					break;
				default:
					_logger.LogError("Invalid templateId: {TemplateId} provided for GetLoanDetailById", templateId);
					break;
			}

			// Join tables
			loanReviewRequest = loanReviewRequest
				.Include(ent => ent.LoanReviewStatus)
				.Include(ent => ent.UserCompletedBy)
				.Include(ent => ent.UserUpdatedBy);

			_logger.LogDebug("Execution plan for GetLoan method has been created.");

			// Filtering
			var loanReviewDetailDto = await loanReviewRequest
				.AsNoTracking()
				.AsSplitQuery()
				.Where(ent => !ent.IsLocked)
				.Select(ent => new LoanReviewDetailDto
				{
					Id = ent.Id,
					LoanId = ent.LoanId,
					Status = (LoanReviewStatuses)ent.StatusId!,
					Director = templateId == (int)ReviewTemplates.Residential
						? ent.LoanDetail!.Director : ent.LoanDetailsComm!.Director,
					BorrowerName = templateId == (int)ReviewTemplates.Residential
						? ent.LoanDetail!.BorrowerName : ent.LoanDetailsComm!.BorrowerName,
					Branch = templateId == (int)ReviewTemplates.Residential
						? ent.LoanDetail!.BranchOfficeId : ent.LoanDetailsComm!.BranchOfficeId,
					ProductCode = templateId == (int)ReviewTemplates.Residential
						? ent.LoanDetail!.ProductBusinessGroupCode : ent.LoanDetailsComm!.ProductBusinessGroupCode,
					ReviewedBy = ent.UserCompletedBy!.UserName,
					MortgageOfficer = templateId == (int)ReviewTemplates.Residential
						? ent.LoanDetail!.MortgageOfficerName : ent.LoanDetailsComm!.MortgageOfficerName,
					Province = templateId == (int)ReviewTemplates.Residential
						? ent.LoanDetail!.Province : ent.LoanDetailsComm!.Province,
					ReviewType = templateId == (int)ReviewTemplates.Residential
						? ent.LoanDetail!.ReviewType : ent.LoanDetailsComm!.ReviewType,
					CompletedBy = ent.UserCompletedBy != null ? ent.UserCompletedBy.UserName : null,
					LastUpdatedBy = ent.UserUpdatedBy != null ? ent.UserUpdatedBy.UserName : null,
					LoanDetail = templateId == (int)ReviewTemplates.Residential
						? new LoanDetailDto
						{
							Id = ent.LoanDetail!.Id,
							BorrowerName = ent.LoanDetail.BorrowerName,
							Province = ent.LoanDetail.Province,
							BranchOfficeId = ent.LoanDetail.BranchOfficeId,
							BranchOfficeText = ent.LoanDetail.BranchOfficeText,
							Director = ent.LoanDetail.Director,
							ProductBusinessGroupCode = ent.LoanDetail.ProductBusinessGroupCode,
							ReviewType = ent.LoanDetail.ReviewType,
							Date1 = ent.LoanDetail.Date1,
							MortgageOfficerName = ent.LoanDetail.MortgageOfficerName,
							Obs = ent.LoanDetail.Obs,
							ScoringValue = ent.LoanDetail.ScoringValue,
							SecurityTypeCode = ent.LoanDetail.SecurityTypeCode,
							SpRating = ent.LoanDetail.SpRating,
							SecurityTypeDescription = ent.LoanDetail.SecurityTypeDescription,
							StressTDS = ent.LoanDetail.StressTDS,
							MortgageOfficerSkey = ent.LoanDetail.MortgageOfficerSkey,
							RiskScore = ent.LoanDetail.RiskScore,
							UnderwriterSkey = ent.LoanDetail.UnderwriterSkey,
							AggRating = ent.LoanDetail.AggRating,
							AmortizationTermInMonths = ent.LoanDetail.AmortizationTermInMonths,
							ApplicantCifNumber = ent.LoanDetail.ApplicantCifNumber,
							Balance = ent.LoanDetail.Balance,
							Beacon = ent.LoanDetail.Beacon,
							BrokerCompany = ent.LoanDetail.BrokerCompany,
							CommitmentDate = ent.LoanDetail.CommitmentDate,
							CreatedAt = ent.LoanDetail.CreatedAt,
							EffectiveRate = ent.LoanDetail.EffectiveRate,
							EmploymentType = ent.LoanDetail.EmploymentType,
							ExceptionFlag = ent.LoanDetail.ExceptionFlag,
							ExceptionType = ent.LoanDetail.ExceptionType,
							FundingDate = ent.LoanDetail.FundingDate,
							Income = ent.LoanDetail.Income,
							InsuranceType = ent.LoanDetail.InsuranceType,
							LoanClass = ent.LoanDetail.LoanClass,
							LoanConformingIndicator = ent.LoanDetail.LoanConformingIndicator,
							LTV = ent.LoanDetail.LTV,
							MaturityDate = ent.LoanDetail.MaturityDate,
							MortgageOfficerBp = ent.LoanDetail.MortgageOfficerBp,
							NonConformingReason = ent.LoanDetail.NonConformingReason,
							Occupation = ent.LoanDetail.Occupation,
							OriginalTdsRatio = ent.LoanDetail.OriginalTdsRatio,
							PartnerName = ent.LoanDetail.PartnerName,
							PriorEncAmount = ent.LoanDetail.PriorEncAmount,
							PartnerId = ent.LoanDetail.PartnerId,
							PriorEncCount = ent.LoanDetail.PriorEncCount,
							ProductText = ent.LoanDetail.ProductText,
							Purpose = ent.LoanDetail.Purpose,
							PrRating = ent.LoanDetail.PrRating,
							UpdatedAt = ent.LoanDetail.UpdatedAt,
							ReasonCode = ent.LoanDetail.ReasonCode,
							TermInMonths = ent.LoanDetail.TermInMonths,
							UserName = ent.LoanDetail.UserName,
							VariableRateFlag = ent.LoanDetail.VariableRateFlag
						}
						:
						new LoanDetailDto
						{
							Id = ent.LoanDetailsComm!.Id,
							BorrowerName = ent.LoanDetailsComm.BorrowerName,
							Province = ent.LoanDetailsComm.Province,
							BranchOfficeId = ent.LoanDetailsComm.BranchOfficeId,
							BranchOfficeText = ent.LoanDetailsComm.BranchOfficeText,
							Director = ent.LoanDetailsComm.Director,
							ProductBusinessGroupCode = ent.LoanDetailsComm.ProductBusinessGroupCode,
							ReviewType = ent.LoanDetailsComm.ReviewType,
							Date1 = ent.LoanDetailsComm.Date1,
							MortgageOfficerName = ent.LoanDetailsComm.MortgageOfficerName,
							Obs = ent.LoanDetailsComm.Obs,
							ScoringValue = ent.LoanDetailsComm.ScoringValue,
							SecurityTypeCode = ent.LoanDetailsComm.SecurityTypeCode,
							SpRating = ent.LoanDetailsComm.SpRating,
							SecurityTypeDescription = ent.LoanDetailsComm.SecurityTypeDescription,
							StressTDS = ent.LoanDetailsComm.StressTDS,
							MortgageOfficerSkey = ent.LoanDetailsComm.MortgageOfficerSkey,
							RiskScore = ent.LoanDetailsComm.RiskScore,
							UnderwriterSkey = ent.LoanDetailsComm.UnderwriterSkey,
							AggRating = ent.LoanDetailsComm.AggRating,
							AmortizationTermInMonths = ent.LoanDetailsComm.AmortizationTermInMonths,
							ApplicantCifNumber = ent.LoanDetailsComm.ApplicantCifNumber,
							Balance = ent.LoanDetailsComm.Balance,
							Beacon = ent.LoanDetailsComm.Beacon,
							BrokerCompany = ent.LoanDetailsComm.BrokerCompany,
							CommitmentDate = ent.LoanDetailsComm.CommitmentDate,
							CreatedAt = ent.LoanDetailsComm.CreatedAt,
							EffectiveRate = ent.LoanDetailsComm.EffectiveRate,
							EmploymentType = ent.LoanDetailsComm.EmploymentType,
							ExceptionFlag = ent.LoanDetailsComm.ExceptionFlag,
							ExceptionType = ent.LoanDetailsComm.ExceptionType,
							FundingDate = ent.LoanDetailsComm.FundingDate,
							Income = ent.LoanDetailsComm.Income,
							InsuranceType = ent.LoanDetailsComm.InsuranceType,
							LoanClass = ent.LoanDetailsComm.LoanClass,
							LoanConformingIndicator = ent.LoanDetailsComm.LoanConformingIndicator,
							LTV = ent.LoanDetailsComm.LTV,
							MaturityDate = ent.LoanDetailsComm.MaturityDate,
							MortgageOfficerBp = ent.LoanDetailsComm.MortgageOfficerBp,
							NonConformingReason = ent.LoanDetailsComm.NonConformingReason,
							Occupation = ent.LoanDetailsComm.Occupation,
							OriginalTdsRatio = ent.LoanDetailsComm.OriginalTdsRatio,
							PartnerName = ent.LoanDetailsComm.PartnerName,
							PriorEncAmount = ent.LoanDetailsComm.PriorEncAmount,
							PartnerId = ent.LoanDetailsComm.PartnerId,
							PriorEncCount = ent.LoanDetailsComm.PriorEncCount,
							ProductText = ent.LoanDetailsComm.ProductText,
							Purpose = ent.LoanDetailsComm.Purpose,
							PrRating = ent.LoanDetailsComm.PrRating,
							UpdatedAt = ent.LoanDetailsComm.UpdatedAt,
							ReasonCode = ent.LoanDetailsComm.ReasonCode,
							TermInMonths = ent.LoanDetailsComm.TermInMonths,
							UserName = ent.LoanDetailsComm.UserName,
							VariableRateFlag = ent.LoanDetailsComm.VariableRateFlag
						}
				})
				.FirstOrDefaultAsync(ent => ent.LoanId == loanId, cancellationToken);

			if (loanReviewDetailDto == null)
			{
				_logger.LogWarning("Loan Review doesn't exists with provided LoanId = {LoanId}", loanId);

				return new LoanReviewDetailDto();
			}

			_logger.LogDebug("Loan Review found with LoanId = {LoanId}. Fetching Loan Review Items.", loanId);

			var loanReviewItems = await dbContext.LoanReviewItems
				.AsNoTracking()
				.AsSplitQuery()
				.Include(ent => ent.ReviewOption)
				.Include(ent => ent.TemplateItem)
					.ThenInclude(ent => ent!.ReviewTemplate)
				.Where(ent => ent.ReviewId == loanReviewDetailDto!.Id)
				.Select(ent => new LoanReviewItemsDto
				{
					Id = ent.Id,
					Comment = ent.Comment,
					CreatedAt = ent.CreatedAt,
					UpdatedAt = ent.UpdatedAt,
					ReviewOption = new ReviewOptionDto
					{
						Id = ent.ReviewOption!.Id,
						OptionText = ent.ReviewOption.Name
					},
					TemplateItem = new ReviewTemplateItemDto
					{
						Id = ent.TemplateItem!.Id,
						CreatedAt = ent.TemplateItem.CreatedAt,
						HasComment = ent.TemplateItem.HasComment,
						UpdatedAt = ent.TemplateItem.UpdatedAt,
						OptionType = new ReviewOptionTypeDto
						{
							Id = ent.TemplateItem.ReviewTemplate!.Id,
							OptionName = ent.TemplateItem.ReviewTemplate.Name
						}
					}
				})
				.ToListAsync(cancellationToken);

			if (loanReviewItems.Any())
			{
				loanReviewDetailDto.LoanReviewItems!.AddRange(loanReviewItems);
			}
			else
			{
				_logger.LogInformation("No Loan Review Items found for Loan Review Id = {LoanReviewId}", loanReviewDetailDto.Id);	
			}

			_logger.LogDebug("Loan Review Items fatched successfully. Caching entire entity value.");

			await _hybridCache.SetAsync(loanCacheKey, loanReviewDetailDto, cancellationToken: cancellationToken);

			return loanReviewDetailDto!;
		}

		public async Task<SearchResponseDto> Search(SearchRequestDto searchRequest, CancellationToken cancellationToken)
		{
			using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

			var generalQuery = dbContext.LoanReviewDetails.AsQueryable();

			if (searchRequest.TemplateId == (int)ReviewTemplates.Residential)
			{
				generalQuery = generalQuery.Include(ent => ent.LoanDetail)
					.Where(ent => ent.LoanDetail != null && ent.LoanDetail.IsLocked != 1);
			}
			else
			{
				generalQuery = generalQuery.Include(ent => ent.LoanDetailsComm)
					.Where(ent => ent.LoanDetailsComm != null && ent.LoanDetailsComm.IsLocked != 1);
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
				.Where(ent => !ent.IsLocked)
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
					generalQuery = generalQuery.Include(ent => ent.LoanDetail)
						.Where(ent => ent.LoanDetail != null && ent.LoanDetail.IsLocked != 1);

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
					generalQuery = generalQuery.Include(ent => ent.LoanDetailsComm)
						.Where(ent => ent.LoanDetailsComm != null && ent.LoanDetailsComm.IsLocked != 1);

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
