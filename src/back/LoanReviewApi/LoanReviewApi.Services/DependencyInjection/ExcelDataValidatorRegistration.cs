using LoanReviewApi.Services.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace LoanReviewApi.Services.DependencyInjection
{
    public static class ExcelDataValidatorRegistration
    {
        public static IServiceCollection AddExcelValidators(this IServiceCollection services)
        {
            services.AddScoped<IExcelRowValidator, ExcelRowValidator>();

            return services;
        }
    }
}
