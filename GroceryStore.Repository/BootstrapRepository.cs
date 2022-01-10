using GroceryStore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroceryStore.Repository
{
    /// <summary>
    /// Bootstrap class for adding extension methods related to the Repository layer.
    /// </summary>
    public static class BootstrapRepository
    {
        /// <summary>
        /// Adds services to container for Repository project.
        /// </summary>
        /// <param name="serviceCollection">An instance of <see cref="IServiceCollection"/></param>
        /// <param name="configuration">An instance of <see cref="IConfiguration"/></param>
        public static void AddRepositoryToServiceCollection(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<ICustomerRepository, CustomerRepository>();

            serviceCollection.Configure<DataStoreSettings>(cfg=> configuration.GetSection("DataStoreSettings").Bind(cfg));
        }
    }
}
