using LoanReviewApi.Services.Import;
using LoanReviewApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LoanReviewApi.Services.DependencyInjection
{
    public static class ExcelImportServiceRegistration
    {
        public static IServiceCollection AddExcelImportService(this IServiceCollection services)
        {
            services.AddScoped<IImportLoansService, ImportExcelService>();

            return services;
        }
    }
}
