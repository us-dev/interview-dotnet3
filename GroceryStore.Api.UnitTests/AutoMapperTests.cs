using GroceryStoreAPI.Bootstrap;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GroceryStore.Api.UnitTests
{
    public class AutoMapperTests
    {
        [Fact]
        public void AutoMapperTest()
        {
            IServiceCollection services = new ServiceCollection();

            var autoMapperProfiles = new List<Profile>()
            {
                new ControllerAutoMapperProfile()
            };

            services.AddAutoMapper(config =>
            {
                config.AddProfiles(autoMapperProfiles);
            });

            var serviceProvider = services.BuildServiceProvider();

            var mapperInstance = serviceProvider.GetService<IMapper>();

            mapperInstance.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
