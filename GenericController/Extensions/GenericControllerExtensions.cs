using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace GenericController.API.Extensions
{
    public static class GenericControllerExtensions
    {
        //ref: https://github.com/matjazmav/generic-api
        public static IMvcBuilder AddGenericControllers<TDbContext, TEntity>(this IMvcBuilder mvcBuilder, Type genericControllerType) where TDbContext : DbContext
        {
            var entityTypes = typeof(TDbContext)
                .GetProperties()
                .Select(p => p.PropertyType)
                .Where(pt => pt.IsGenericType
                    && pt.GetGenericTypeDefinition() == typeof(DbSet<>))
                .Select(pt => pt.GetGenericArguments()[0])
                .Where(t => typeof(TEntity).IsAssignableFrom(t)
                    && t.IsClass
                    && !t.IsAbstract);

            mvcBuilder.ConfigureApplicationPartManager(manager =>
            {
                manager.ApplicationParts.Add(new GenericControllerApplicationPart(genericControllerType, entityTypes));
            });

            return mvcBuilder;
        }
    }
}
