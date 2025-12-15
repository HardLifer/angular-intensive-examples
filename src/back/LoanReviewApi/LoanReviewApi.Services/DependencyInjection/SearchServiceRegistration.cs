using LoanReviewApi.Services.Interfaces;
using LoanReviewApi.Services.Search;
using Microsoft.Extensions.DependencyInjection;

namespace LoanReviewApi.Services.DependencyInjection
{
    public static class SearchServiceRegistration
    {
        public static IServiceCollection AddSearchLoanService(this IServiceCollection services)
        {
            services.AddScoped<ISearchLoanService, SearchLoanService>();

            return services;
        }
    }
}
