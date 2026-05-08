using ePizza.Domain.Interfaces;
using ePizza.Infrastructure.Entities;
using ePizza.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ePizza.Infrastructure
{
    public static class ServiceRegistration
    {
        //extension method to extend IServiceCollection 

        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,string connectionstring)
        {
            services.AddDbContext<ePizzaDbContext>(options =>
             {
                 options.UseSqlServer(connectionstring);

              });

            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            //services.AddScoped<IUserTokenRepository, UserTokenRepository>();

            return services;
        }
    }
}
