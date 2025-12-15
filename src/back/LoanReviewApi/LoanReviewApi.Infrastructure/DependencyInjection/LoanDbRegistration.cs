using LoanReviewApi.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LoanReviewApi.Infrastructure.DependencyInjection
{
    public static class LoanDbRegistration
    {
        public static IServiceCollection AddLoanDbContext(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddDbContextFactory<LoanReviewContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlConnection"), sqlServerOptions =>
                {
                    sqlServerOptions.CommandTimeout(10);
                    sqlServerOptions.EnableRetryOnFailure(3);
                });

                if (environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
            });

            services.AddDbContext<LoanReviewContext>();

            return services;
        }
    }
}
