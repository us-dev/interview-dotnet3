using GroceryStoreAPI.Bootstrap;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using GroceryStore.Models;
using GroceryStore.Services;
using GroceryStoreAPI.ApiRequestModel;
using GroceryStoreAPI.ApiResponseModel;
using GroceryStoreAPI.Controllers;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GroceryStore.Api.UnitTests
{
    public class CustomerControllerTests
    {
        private Mock<ILogger<CustomerController>> LoggerMock { get; set; }

        private Mock<ICustomerService> CustomerServiceMock { get; set; }

        public IMapper Mapper { get; set; }

        public CustomerController CustomerController { get; set; }

        public Fixture Fixture { get; set; }

        public CustomerControllerTests()
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

            this.Mapper = serviceProvider.GetService<IMapper>();

            this.Fixture = new Fixture();

            this.LoggerMock = new Mock<ILogger<CustomerController>>();

            this.CustomerServiceMock = new Mock<ICustomerService>();

            this.CustomerController = new CustomerController(
                this.LoggerMock.Object,
                this.CustomerServiceMock.Object,
                this.Mapper);
        }

        [Fact]
        public async Task GetAllCustomerReturnsList()
        {
            var customersToReturn = this.Fixture.CreateMany<CustomerModel>(5).ToList();
            this.CustomerServiceMock.Setup(x => x.GetAllCustomers()).ReturnsAsync(customersToReturn);

            var result = await this.CustomerController.GetAllCustomer();

            Assert.NotNull(result);
            Assert.Equal(customersToReturn.Count, result.Count);
        }

        [Fact]
        public async Task GetCustomerByIdReturnsACustomer()
        {
            var customersToReturn = this.Fixture.CreateMany<CustomerModel>(5).ToList();
            var customerIdToUse = customersToReturn[0].Id;

            this.CustomerServiceMock.Setup(x => x.GetCustomerById(customerIdToUse)).ReturnsAsync(customersToReturn[0]);

            var result = await this.CustomerController.GetCustomerById(customerIdToUse);
            var response = result.Result as OkObjectResult;

            Assert.NotNull(response.Value);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.Equal(customerIdToUse, ((CustomerModel)response.Value).Id);
            Assert.Equal(customersToReturn[0].Name, ((CustomerModel)response.Value).Name);
        }

        [Fact]
        public async Task GetCustomerByIdReturnsEmptyData()
        {
            this.CustomerServiceMock.Setup(x => x.GetCustomerById(It.IsAny<long>())).ReturnsAsync((CustomerModel)null);

            var result = await this.CustomerController.GetCustomerById(1);
            var response = result.Result as NotFoundObjectResult;

            Assert.NotNull(response.Value);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound.ToString(), ((BadRequestModel)response.Value).Error);
        }

        [Fact]
        public async Task CreateCustomerReturnsACustomer()
        {
            var customerToCreate = this.Fixture.Create<CustomerRequestModel>();
            var customerToReturn = new CustomerModel
            {
                Id = 1,
                Name = customerToCreate.Name
            };

            this.CustomerServiceMock.Setup(x => x.AddCustomer(It.IsAny<CustomerModel>())).ReturnsAsync(customerToReturn);

            var result = await this.CustomerController.CreateCustomer(customerToCreate);
            var response = result.Result as ObjectResult;

            Assert.NotNull(response.Value);
            Assert.Equal(StatusCodes.Status201Created, response.StatusCode);
        }

        [Fact]
        public async Task UpdateCustomerReturnsACustomer()
        {
            var customerToCreate = this.Fixture.Create<CustomerModel>();

            this.CustomerServiceMock.Setup(x => x.UpdateCustomer(It.IsAny<CustomerModel>())).ReturnsAsync(customerToCreate);

            var result = await this.CustomerController.UpdateCustomer(customerToCreate);
            var response = result.Result as ObjectResult;

            Assert.NotNull(response.Value);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteCustomerReturnsACustomer()
        {
            var customerIdToReturn = this.Fixture.Create<long>();

            this.CustomerServiceMock.Setup(x => x.DeleteCustomer(It.IsAny<long>())).ReturnsAsync(customerIdToReturn);

            var result = await this.CustomerController.DeleteCustomer(customerIdToReturn);
            var response = result as StatusCodeResult;

            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }
    }
}
