using FluentValidation;
using LoanReviewApi.Validators;

namespace LoanReviewApi.DependencyInjection
{
	public static class ValidatorsRegistration
	{
		public static IServiceCollection AddEndpointRequestValidators(this IServiceCollection services)
		{
			services.AddValidatorsFromAssemblyContaining<SearchRequestValidator>(ServiceLifetime.Scoped);

			return services;
		}
	}
}
