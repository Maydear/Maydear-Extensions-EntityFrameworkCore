using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maydear.Extensions.EntityFrameworkCore.Test
{
    public static class ServiceCollectionFactory
    {
        private static IServiceCollection serviceCollection;

        public static T GetRequiredService<T>() where T : class
        {
            InitServiceCollection();
            return serviceCollection.BuildServiceProvider().GetRequiredService<T>();
        }

        private static void InitServiceCollection()
        {
            if (serviceCollection == null)
            {
                serviceCollection = new ServiceCollection();
                serviceCollection.AddRepositories();
            }
        }
    }
}
