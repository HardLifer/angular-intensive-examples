namespace LoanReviewApi.Services.Models
{
    public class FailedImportLoanData
    {
        public int RowNumber { get; set; } = 0;

        public string? ColumnName { get; set; } = string.Empty;

        public string? RowData { get; set; } = string.Empty;

        public bool IsValid { get; set; } = true;

        public string ValidationMessage { get; set; } = string.Empty;
    }
}
