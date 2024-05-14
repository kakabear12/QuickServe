using QuickServe.Application.Interfaces;
using QuickServe.WebAPI.Services;
using QuickServe.Infrastructure;
namespace QuickServe.WebAPI
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddWebAPIService(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IClaimsService, ClaimsService>();


            services.AddHttpContextAccessor();
            return services;
        }

    }
}
