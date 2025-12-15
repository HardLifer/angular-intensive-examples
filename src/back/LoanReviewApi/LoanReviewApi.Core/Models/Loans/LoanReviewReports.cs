namespace LoanReviewApi.Core.Models.Loans
{
    public class LoanReviewReports
    {
        public int Id { get; set; }

        public int? L1L2Count { get; set; }

        public int? L1Count { get; set; }

        public int? L2Count { get;set; }

        public int? L2TDSIncCount { get; set; }

        public int? L2OtherCount { get; set; }

        public int? CDCount { get; set; }

        public int? RefCompCount {  get; set; }

        public int? DecisionCount { get; set; }
    }
}
