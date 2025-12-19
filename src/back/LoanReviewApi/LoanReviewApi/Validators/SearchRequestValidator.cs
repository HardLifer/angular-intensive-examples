using FluentValidation;
using LoanReviewApi.Services.DTOs;
using LoanReviewApi.Services.Models;

namespace LoanReviewApi.Validators
{
	public class SearchRequestValidator : AbstractValidator<SearchRequestDto>
	{
		public SearchRequestValidator()
		{
			RuleFor(ent => ent.Pagination).NotNull().WithMessage("Pagination is required");
			RuleFor(ent => ent.TemplateId).GreaterThan(0).WithMessage("TemplateId must be greater than 0");

			RuleFor(ent => ent.Pagination).SetValidator(new PaginationValidator());
		}
	}

	public class PaginationValidator : AbstractValidator<Pagination>
	{
		public PaginationValidator()
		{
			RuleFor(ent => ent.PageNumber).GreaterThan(0).WithMessage("PageNumber must be greater than 0");
			RuleFor(ent => ent.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0");
		}
	}
}
