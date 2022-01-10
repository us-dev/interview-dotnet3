using GroceryStore.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroceryStore.Services
{
    /// <summary>
    /// Bootstrap class for adding extension methods related to the Services layer.
    /// </summary>
    public static class BootstrapServices
    {
        /// <summary>
        /// Adds services to container for Services project.
        /// </summary>
        /// <param name="serviceCollection">An instance of <see cref="IServiceCollection"/></param>
        /// <param name="configuration">An instance of <see cref="IConfiguration"/></param>
        public static void AddServicesToServiceCollection(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<ICustomerService, CustomerService>();

            serviceCollection.AddRepositoryToServiceCollection(configuration);
        }
    }
}
