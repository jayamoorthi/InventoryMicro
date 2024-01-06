﻿using ProductApi.Domain.Interfaces;
using ProductApi.Domain.Services;
using ProductApi.Infrastructure.Interfaces;
using ProductApi.Infrastructure.Repositories;

namespace kafka.ProductApi.ConfigurationExtension
{
    public static class SericeConfigurationExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IInventoryRepository, InventoryRepository>();            

        }
    }
}
