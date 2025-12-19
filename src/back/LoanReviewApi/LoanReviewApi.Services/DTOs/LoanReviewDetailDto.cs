using LoanReviewApi.Core.Enums;

namespace LoanReviewApi.Services.DTOs
{
	public class LoanReviewDetailDto
	{
		public int Id { get; set; }

		public int LoanId { get; set; }

		public string? BorrowerName { get; set; }

		public DateTime? Date { get; set; }

		public LoanReviewStatuses Status { get; set; }

		public string? Director { get; set; }

		public string? Branch { get; set; }

		public string? ProductCode { get; set; }

		public string? ReviewedBy { get; set; }

		public string? MortgageOfficer { get; set; }

		public string? Province { get; set; }

		public string? ReviewType { get; set; }

		public string? CompletedBy { get; set; }

		public string? LastUpdatedBy { get; set; }

		public LoanDetailDto? LoanDetail { get; set; }

		public List<LoanReviewItemsDto>? LoanReviewItems { get; set; } = new List<LoanReviewItemsDto>();

	}

	public class LoanReviewItemsDto
	{
		public int Id { get; set; }

		public string? Comment { get; set; }

		public ReviewOptionDto? ReviewOption { get; set; }

		public ReviewTemplateItemDto? TemplateItem { get; set; }

		public DateTime? CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }
	}

	public class ReviewOptionDto
	{
		public int Id { get; set; }

		public string? OptionText { get; set; }
	}

	public class ReviewTemplateItemDto
	{
		public int Id { get; set; }

		public bool HasComment { get; set; }

		public ReviewOptionTypeDto? OptionType { get; set; }

		public DateTime? CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

	}

	public class ReviewOptionTypeDto
	{
		public int Id { get; set; }

		public string? OptionName { get; set; }

		public DateTime? CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }
	}

	public class LoanDetailDto
	{
		public int Id { get; set; }

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

		public DateTime? CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }
	}
}
