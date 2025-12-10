using System.ComponentModel.DataAnnotations;

namespace LoanReviewApi.Core.Models.Loans
{
    public class LoanDetail
    {
        public LoanDetail()
        {
            LoanReviewDetails = new List<LoanReviewDetail>();
        }

        public int Id { get; set; }

        public int LoanId { get; set; }

        [Timestamp]
        public byte[] SSMA_TimeStamp { get; set; } = null!;

        public float? Obs { get; set; }

        public string? ProductBusinessGroupCode { get; set; }

        public string? LoanClass { get; set; }

        public string? InsuranceType { get; set; }

        public string? VariableRateFlag { get; set; }

        public string? Province { get; set; }

        public float? LTV { get; set; }

        public float? Balance { get; set; }

        public string? Purpose { get; set; }

        public float? Beacon { get; set; }

        public float? RiskScore { get; set; }

        public float? AmortizationTermInMonths { get; set; }

        public float? TermInMonths { get; set; }

        public float? SecurityTypeCode { get; set; }

        public string? SecurityTypeDescription { get; set; }

        public string? CommitmentDate { get; set; }

        public string? MaturityDate { get; set; }

        public string? ApplicantCifNumber { get; set; }

        public string? BrokerCompany { get; set; }

        public float? EffectiveRate { get; set; }

        public string? ExceptionFlag { get; set; }

        public string? ExceptionType { get; set; }

        public float? OriginalTdsRatio { get; set; }

        public string? LoanConformingIndicator { get; set; }

        public float? ReasonCode { get; set; }

        public float? ScoringValue { get; set; }

        public string? EmploymentType { get; set; }

        public string? Occupation { get; set; }

        public float? Income { get; set; }

        public string? ProductText { get; set; }

        public string? FundingDate { get; set; }

        public float? PriorEncCount { get; set; }

        public float? PriorEncAmount { get; set; }

        public string? BranchOfficeId { get; set; }

        public string? BranchOfficeText { get; set; }

        public string? BorrowerName { get; set; }

        public string? UnderwriterSkey { get; set; }

        public string? PartnerName { get; set; }

        public string? PartnerId { get; set; }

        public string? Director { get; set; }

        public DateTime? Date1 { get; set; }

        public string? ReviewType { get; set; }

        public string? NonConformingReason { get; set; }

        public string? MortgageOfficerBp { get; set; }

        public string? MortgageOfficerName { get; set; }

        public string? MortgageOfficerSkey { get; set; }

        public string? AggRating { get; set; }

        public string? PrRating { get; set; }

        public string? SpRating { get; set; }

        public string? UserName { get; set; }

        public float? StressTDS { get; set; }

        public byte? IsLocked { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public ICollection<LoanReviewDetail> LoanReviewDetails { get; set; }
    }
}
