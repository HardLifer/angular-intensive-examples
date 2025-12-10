using LoanReviewApi.Services.Models;
using System.Data;

namespace LoanReviewApi.Services.Validators
{
    public interface IExcelRowValidator
    {
        FailedImportLoanData Validate(DataRow row, int rowNumber, Dictionary<int, string> headersLookup);
    }

    public class ExcelRowValidator : IExcelRowValidator
    {
        public FailedImportLoanData Validate(DataRow row, int rowNumber, Dictionary<int, string> headersLookup)
        {
            var failedLoans = new List<FailedImportLoanData>();

            if (!int.TryParse(row[1]?.ToString(), out int _))
            {
                var failedLoan = new FailedImportLoanData();

                failedLoan.IsValid = false;
                failedLoan.ColumnName = headersLookup[1];
                failedLoan.RowNumber = rowNumber;
                failedLoan.RowData = row[1]?.ToString();
                failedLoan.ValidationMessage = $"Loan Code doesn't provide for row = '{rowNumber}'";

                return failedLoan;
            }

            if (int.TryParse(row[8]?.ToString(),out int balance) && balance < 0)
            {
                var failedBalance = new FailedImportLoanData();

                failedBalance.IsValid = false;
                failedBalance.ColumnName = headersLookup[rowNumber - 1];
                failedBalance.RowNumber = rowNumber;
                failedBalance.RowData = balance.ToString();
                failedBalance.ValidationMessage = $"Balance is negative for row = '{rowNumber}'";

                return failedBalance;
            }

            return null!;
        }
    }
}
