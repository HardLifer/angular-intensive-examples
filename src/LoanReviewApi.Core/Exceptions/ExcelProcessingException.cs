namespace LoanReviewApi.Core.Exceptions
{
    public class ExcelProcessingException : Exception
    {
        public int RowNumber { get; set; }

        public string? ColumnName { get; set; }

        public ExcelProcessingException(string message)
            : base(message)
        {
        }

        public ExcelProcessingException(string message, Exception innerEception)
            : base(message, innerEception)
        {
        }

        public ExcelProcessingException(string message, int rowNumber, string? columnName = null)
            : base(message)
        {
            RowNumber = rowNumber;
            ColumnName = columnName;
        }
    }

    public class InvalidCellDataException : ExcelProcessingException
    {
        public InvalidCellDataException(string message, int rowNumber, string columnName)
            : base(message, rowNumber, columnName)
        {
        }
    }

    public class RequiredDataMissingException : ExcelProcessingException
    {
        public RequiredDataMissingException(string message, int rowNumber, string columnName)
            : base(message, rowNumber, columnName)
        {
        }
    }
}
