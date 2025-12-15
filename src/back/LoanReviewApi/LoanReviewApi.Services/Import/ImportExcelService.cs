using ExcelDataReader;
using LoanReviewApi.Core.Enums;
using LoanReviewApi.Core.Exceptions;
using LoanReviewApi.Core.Models.Loans;
using LoanReviewApi.Infrastructure.Context;
using LoanReviewApi.Services.Interfaces;
using LoanReviewApi.Services.Models;
using LoanReviewApi.Services.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text;

namespace LoanReviewApi.Services.Import
{
    public class ImportExcelService : IImportLoansService
    {
        private readonly IDbContextFactory<LoanReviewContext> _dbContextFactory;
        private readonly ILogger<ImportExcelService> _logger;
        private readonly IExcelRowValidator _rowValidator;

        public ImportExcelService(IDbContextFactory<LoanReviewContext> dbContextFactory, ILogger<ImportExcelService> logger, IExcelRowValidator rowValidator)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
            _rowValidator = rowValidator;
        }

        public async Task<IEnumerable<FailedImportLoanData>> ImportLoans(IFormFile file, int templateId, CancellationToken cancellationToken)
        {
            DataSet? data = null;
            List<FailedImportLoanData> failedLoans = new List<FailedImportLoanData>();
            List<LoanDetail> loans = new List<LoanDetail>();
            List<LoanDetailsComm> loanDetailsComms = new List<LoanDetailsComm>();
            Dictionary<int, string> headersLookup = null!;

            try
            {
                using (var excelFileStream = file.OpenReadStream())
                {
                    using (var excelReader = ExcelReaderFactory.CreateReader(excelFileStream))
                    {
                        data = await Task.Run(() => excelReader.AsDataSet(), cancellationToken).ConfigureAwait(false);

                        if (data == null || data.Tables.Count == 0)
                        {
                            throw new ExcelProcessingException("Excel file doesn't contain any sheets.");
                        }

                        // Process the first sheet ( we are assuing that document contains only one and doesn't support multi-sheet )
                        var dataTable = data.Tables[0];

                        if (dataTable == null || dataTable.Rows.Count == 0)
                        {
                            throw new ExcelProcessingException("Excel sheet is empty");
                        }

                        int index = 0;
                        headersLookup = dataTable.Rows[0].ItemArray.ToDictionary(key => index++, elem => elem?.ToString())!;

                        switch (templateId)
                        {
                            case (int)ReviewTemplates.Residential:
                                {
                                    (var failedImportLoans, var successImportedLoans) = ProccessResedentialExcelDataTable(failedLoans, loans, dataTable, headersLookup);

                                    if (failedImportLoans.Count > 0)
                                    {
                                        return failedImportLoans;
                                    }

                                    var duplicateLoans = await InsertResedentialLoansToDatabase(successImportedLoans, cancellationToken);

                                    if (duplicateLoans != null)
                                    {
                                        failedImportLoans.Add(duplicateLoans);

                                        return failedImportLoans;
                                    }
                                }
                                break;
                            case (int)ReviewTemplates.Commercial:
                                {
                                    (var failedImportLoans, var successImportedLoans) = ProccessCommercialExcelDataTable(failedLoans, loanDetailsComms, dataTable, headersLookup);

                                    if (failedImportLoans.Count > 0)
                                    {
                                        return failedImportLoans;
                                    }

                                    var duplicateLoans = await InsertCommercialLoansToDatabase(successImportedLoans, cancellationToken);

                                    if (duplicateLoans != null)
                                    {
                                        failedImportLoans.Add(duplicateLoans);

                                        return failedImportLoans;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }

                return failedLoans;
            }
            catch (ExcelProcessingException ePex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                data?.Dispose();
            }
        }

        private (List<FailedImportLoanData> failedLoans, List<LoanDetail> loans) ProccessResedentialExcelDataTable(List<FailedImportLoanData> initialFailedLoans, List<LoanDetail> initialLoans, DataTable data, Dictionary<int, string> headers)
        {
            for (int i = 1; i < data.Rows.Count; i++)
            {
                // validation each cell and row
                var failedLoan = _rowValidator.Validate(data.Rows[i], i, headers);

                if (failedLoan != null)
                {
                    initialFailedLoans.Add(failedLoan);
                }
                else
                {
                    LoanDetail loan = new LoanDetail();
                    var createdTime = DateTimeOffset.UtcNow.DateTime;

                    loan.Obs = float.TryParse(data.Rows[i][0]?.ToString(), out var obsResult) ? obsResult : 0;
                    loan.LoanId = int.TryParse(data.Rows[i][1]?.ToString(), out var loanIdresult) ? loanIdresult : throw new ExcelProcessingException($"Loan Id doesn't provided for the row = {i}", i, headers[i]);
                    loan.ProductBusinessGroupCode = data.Rows[i][2]?.ToString();
                    loan.LoanClass = data.Rows[i][3]?.ToString();
                    loan.InsuranceType = data.Rows[i][4]?.ToString();
                    loan.VariableRateFlag = data.Rows[i][5]?.ToString();
                    loan.Province = data.Rows[i][6]?.ToString();
                    loan.LTV = float.TryParse(data.Rows[i][7]?.ToString(), out var ltvResult) ? ltvResult : 0;
                    loan.Balance = float.TryParse(data.Rows[i][8]?.ToString(), out var balanceResult) ? balanceResult : 0;
                    loan.Purpose = data.Rows[i][9]?.ToString();
                    loan.Beacon = float.TryParse(data.Rows[i][10]?.ToString(), out var beaconResult) ? beaconResult : 0;
                    loan.RiskScore = float.TryParse(data.Rows[i][11]?.ToString(), out var riskScoreResult) ? riskScoreResult : 0;
                    loan.AmortizationTermInMonths = float.TryParse(data.Rows[i][12]?.ToString(), out var amortizationResult) ? amortizationResult : 0;
                    loan.TermInMonths = float.TryParse(data.Rows[i][13]?.ToString(), out var termResult) ? termResult : 0;
                    loan.SecurityTypeCode = float.TryParse(data.Rows[i][14]?.ToString(), out var secCodeResult) ? secCodeResult : 0;
                    loan.SecurityTypeDescription = data.Rows[i][15]?.ToString();
                    loan.CommitmentDate = data.Rows[i][16]?.ToString();
                    loan.MaturityDate = data.Rows[i][17]?.ToString();
                    loan.ApplicantCifNumber = data.Rows[i][18]?.ToString();
                    loan.BrokerCompany = data.Rows[i][19]?.ToString(); ;
                    loan.EffectiveRate = float.TryParse(data.Rows[i][20]?.ToString(), out var effectiveResult) ? effectiveResult : 0;
                    loan.ExceptionFlag = data.Rows[i][21]?.ToString();
                    loan.ExceptionType = data.Rows[i][22]?.ToString();
                    loan.OriginalTdsRatio = float.TryParse(data.Rows[i][23]?.ToString(), out var tdsResult) ? tdsResult : 0;
                    loan.LoanConformingIndicator = data.Rows[i][24]?.ToString();
                    loan.ReasonCode = float.TryParse(data.Rows[i][25]?.ToString(), out var reasonResult) ? reasonResult : 0;
                    loan.ScoringValue = float.TryParse(data.Rows[i][26]?.ToString(), out var scoringResult) ? scoringResult : 0;
                    loan.EmploymentType = data.Rows[i][27]?.ToString();
                    loan.Occupation = data.Rows[i][28]?.ToString();
                    loan.Income = float.TryParse(data.Rows[i][29]?.ToString(), out var incomeResult) ? incomeResult : 0;
                    loan.ProductText = data.Rows[i][30]?.ToString();
                    loan.FundingDate = data.Rows[i][31]?.ToString();
                    loan.PriorEncCount = float.TryParse(data.Rows[i][32]?.ToString(), out var priorCountResult) ? priorCountResult : 0;
                    loan.PriorEncAmount = float.TryParse(data.Rows[i][33]?.ToString(), out var priorAmountResult) ? priorAmountResult : 0;
                    loan.BranchOfficeId = data.Rows[i][34]?.ToString();
                    loan.BranchOfficeText = data.Rows[i][35]?.ToString();
                    loan.BorrowerName = data.Rows[i][36]?.ToString();
                    loan.UnderwriterSkey = data.Rows[i][37]?.ToString();
                    loan.PartnerName = data.Rows[i][38]?.ToString();
                    loan.PartnerId = data.Rows[i][39]?.ToString();
                    loan.Director = data.Rows[i][40]?.ToString();
                    loan.Date1 = DateTime.TryParse(data.Rows[i][41]?.ToString(), out var date1Result) ? date1Result : null;
                    loan.ReviewType = data.Rows[i][42]?.ToString();
                    loan.NonConformingReason = data.Rows[i][43]?.ToString();
                    loan.MortgageOfficerBp = data.Rows[i][44]?.ToString();
                    loan.MortgageOfficerName = data.Rows[i][45]?.ToString();
                    loan.MortgageOfficerSkey = data.Rows[i][46]?.ToString();
                    loan.AggRating = data.Rows[i][47]?.ToString();
                    loan.PrRating = data.Rows[i][48]?.ToString();
                    loan.SpRating = data.Rows[i][49]?.ToString();
                    loan.UserName = data.Rows[i][50]?.ToString();
                    loan.StressTDS = float.TryParse(data.Rows[i][51]?.ToString(), out var stressTdsResult) ? stressTdsResult : 0;

                    loan.LoanReviewDetails.Add(new LoanReviewDetail
                    {
                        StatusId = 3, //New Status
                        CreatedAt = createdTime,
                        IsLocked = false,
                        LoanId = loanIdresult,
                        TemplateId = (int?)ReviewTemplates.Residential,
                    });

                    loan.IsLocked = 0;
                    loan.CreatedAt = createdTime;

                    initialLoans.Add(loan);
                }
            }

            return (initialFailedLoans, initialLoans);
        }

        private (List<FailedImportLoanData> failedLoans, List<LoanDetailsComm> loansComm) ProccessCommercialExcelDataTable(List<FailedImportLoanData> initialFailedLoans, List<LoanDetailsComm> initialLoans, DataTable data, Dictionary<int, string> headers)
        {
            for (int i = 1; i < data.Rows.Count; i++)
            {
                // validation each cell and row
                var failedLoan = _rowValidator.Validate(data.Rows[i], i, headers);

                if (failedLoan != null)
                {
                    initialFailedLoans.Add(failedLoan);
                }
                else
                {
                    LoanDetailsComm loanComm = new LoanDetailsComm();
                    var createdTime = DateTimeOffset.UtcNow.DateTime;

                    loanComm.Obs = float.TryParse(data.Rows[i][0]?.ToString(), out var obsResult) ? obsResult : 0;
                    loanComm.LoanId = int.TryParse(data.Rows[i][1]?.ToString(), out var loanIdresult) ? loanIdresult : throw new ExcelProcessingException($"Loan Id doesn't provided for the row = {i}", i, headers[i]);
                    loanComm.ProductBusinessGroupCode = data.Rows[i][2]?.ToString();
                    loanComm.LoanClass = data.Rows[i][3]?.ToString();
                    loanComm.InsuranceType = data.Rows[i][4]?.ToString();
                    loanComm.VariableRateFlag = data.Rows[i][5]?.ToString();
                    loanComm.Province = data.Rows[i][6]?.ToString();
                    loanComm.LTV = float.TryParse(data.Rows[i][7]?.ToString(), out var ltvResult) ? ltvResult : 0;
                    loanComm.Balance = float.TryParse(data.Rows[i][8]?.ToString(), out var balanceResult) ? balanceResult : 0;
                    loanComm.Purpose = data.Rows[i][9]?.ToString();
                    loanComm.Beacon = float.TryParse(data.Rows[i][10]?.ToString(), out var beaconResult) ? beaconResult : 0;
                    loanComm.RiskScore = float.TryParse(data.Rows[i][11]?.ToString(), out var riskScoreResult) ? riskScoreResult : 0;
                    loanComm.AmortizationTermInMonths = float.TryParse(data.Rows[i][12]?.ToString(), out var amortizationResult) ? amortizationResult : 0;
                    loanComm.TermInMonths = float.TryParse(data.Rows[i][13]?.ToString(), out var termResult) ? termResult : 0;
                    loanComm.SecurityTypeCode = float.TryParse(data.Rows[i][14]?.ToString(), out var secCodeResult) ? secCodeResult : 0;
                    loanComm.SecurityTypeDescription = data.Rows[i][15]?.ToString();
                    loanComm.CommitmentDate = data.Rows[i][16]?.ToString();
                    loanComm.MaturityDate = data.Rows[i][17]?.ToString();
                    loanComm.ApplicantCifNumber = data.Rows[i][18]?.ToString();
                    loanComm.BrokerCompany = data.Rows[i][19]?.ToString(); ;
                    loanComm.EffectiveRate = float.TryParse(data.Rows[i][20]?.ToString(), out var effectiveResult) ? effectiveResult : 0;
                    loanComm.ExceptionFlag = data.Rows[i][21]?.ToString();
                    loanComm.ExceptionType = data.Rows[i][22]?.ToString();
                    loanComm.OriginalTdsRatio = float.TryParse(data.Rows[i][23]?.ToString(), out var tdsResult) ? tdsResult : 0;
                    loanComm.LoanConformingIndicator = data.Rows[i][24]?.ToString();
                    loanComm.ReasonCode = float.TryParse(data.Rows[i][25]?.ToString(), out var reasonResult) ? reasonResult : 0;
                    loanComm.ScoringValue = float.TryParse(data.Rows[i][26]?.ToString(), out var scoringResult) ? scoringResult : 0;
                    loanComm.EmploymentType = data.Rows[i][27]?.ToString();
                    loanComm.Occupation = data.Rows[i][28]?.ToString();
                    loanComm.Income = float.TryParse(data.Rows[i][29]?.ToString(), out var incomeResult) ? incomeResult : 0;
                    loanComm.ProductText = data.Rows[i][30]?.ToString();
                    loanComm.FundingDate = data.Rows[i][31]?.ToString();
                    loanComm.PriorEncCount = float.TryParse(data.Rows[i][32]?.ToString(), out var priorCountResult) ? priorCountResult : 0;
                    loanComm.PriorEncAmount = float.TryParse(data.Rows[i][33]?.ToString(), out var priorAmountResult) ? priorAmountResult : 0;
                    loanComm.BranchOfficeId = data.Rows[i][34]?.ToString();
                    loanComm.BranchOfficeText = data.Rows[i][35]?.ToString();
                    loanComm.BorrowerName = data.Rows[i][36]?.ToString();
                    loanComm.UnderwriterSkey = data.Rows[i][37]?.ToString();
                    loanComm.PartnerName = data.Rows[i][38]?.ToString();
                    loanComm.PartnerId = data.Rows[i][39]?.ToString();
                    loanComm.Director = data.Rows[i][40]?.ToString();
                    loanComm.Date1 = DateTime.TryParse(data.Rows[i][41]?.ToString(), out var date1Result) ? date1Result : null;
                    loanComm.ReviewType = data.Rows[i][42]?.ToString();
                    loanComm.NonConformingReason = data.Rows[i][43]?.ToString();
                    loanComm.MortgageOfficerBp = data.Rows[i][44]?.ToString();
                    loanComm.MortgageOfficerName = data.Rows[i][45]?.ToString();
                    loanComm.MortgageOfficerSkey = data.Rows[i][46]?.ToString();
                    loanComm.AggRating = data.Rows[i][47]?.ToString();
                    loanComm.PrRating = data.Rows[i][48]?.ToString();
                    loanComm.SpRating = data.Rows[i][49]?.ToString();
                    loanComm.UserName = data.Rows[i][50]?.ToString();
                    loanComm.StressTDS = float.TryParse(data.Rows[i][51]?.ToString(), out var stressTdsResult) ? stressTdsResult : 0;

                    loanComm.LoanReviewDetails.Add(new LoanReviewDetail
                    {
                        StatusId = 3, //New Status
                        CreatedAt = createdTime,
                        IsLocked = false,
                        LoanId = loanIdresult,
                        TemplateId = (int?)ReviewTemplates.Commercial,
                    });

                    loanComm.IsLocked = 0;
                    loanComm.CreatedAt = createdTime;

                    initialLoans.Add(loanComm);
                }
            }

            return (initialFailedLoans, initialLoans);
        }

        private async Task<FailedImportLoanData> InsertResedentialLoansToDatabase(IEnumerable<LoanDetail> loans, CancellationToken cancellationToken)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

            var loanIds = loans.Select(ent => ent.LoanId);

            var duplicateResult = await dbContext.LoanReviewDetails
                .AsSplitQuery()
                .Where(ent => loanIds.Contains(ent.LoanId) && ent.TemplateId == (int)ReviewTemplates.Residential)
                .Select(ent => ent.LoanId)
                .ToListAsync(cancellationToken);

            if (duplicateResult.Count == 0)
            {
                await dbContext.LoanDetails.AddRangeAsync(loans);

                await dbContext.SaveChangesAsync(cancellationToken);

                return null!;
            }
            else
            {
                var failedLoanImport = new FailedImportLoanData();
                failedLoanImport.ColumnName = "LoanId";
                failedLoanImport.IsValid = false;
                failedLoanImport.ValidationMessage = new StringBuilder().Append("Next loan ids already exists: ").AppendJoin(',', duplicateResult).ToString();

                return failedLoanImport;
            }
        }

        private async Task<FailedImportLoanData> InsertCommercialLoansToDatabase(IEnumerable<LoanDetailsComm> loanDetailsComms, CancellationToken cancellationToken)
        {
            using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

            var loanIds = loanDetailsComms.Select(ent => ent.LoanId);

            var duplicateResult = await dbContext.LoanReviewDetails
                .AsSplitQuery()
                .Where(ent => loanIds.Contains(ent.LoanId) && ent.TemplateId == (int)ReviewTemplates.Commercial)
                .Select(ent => ent.LoanId)
                .ToListAsync(cancellationToken);

            if (duplicateResult.Count == 0)
            {
                await dbContext.LoanDetailsComm.AddRangeAsync(loanDetailsComms);

                await dbContext.SaveChangesAsync(cancellationToken);

                return null!;
            }
            else
            {
                var failedLoanImport = new FailedImportLoanData();
                failedLoanImport.ColumnName = "LoanId";
                failedLoanImport.IsValid = false;
                failedLoanImport.ValidationMessage = new StringBuilder().Append("Next loan ids already exists: ").AppendJoin(',', duplicateResult).ToString();

                return failedLoanImport;
            }
        }
    }
}
