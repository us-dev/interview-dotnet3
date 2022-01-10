using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Castle.Core.Logging;
using GroceryStore.Models;
using GroceryStore.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GroceryStore.Services.UnitTests
{
    public class CustomerServiceTests
    {
        public Mock<ILogger<CustomerService>> LoggerMock { get; set; }
        public Mock<ICustomerRepository> CustomerRepositoryMock { get; set; }


        public ICustomerService CustomerService { get; set; }

        public Fixture Fixture { get; set; }

        public CustomerServiceTests()
        {
            this.LoggerMock = new Mock<ILogger<CustomerService>>();
            this.CustomerRepositoryMock = new Mock<ICustomerRepository>();

            this.CustomerService = new CustomerService(
                this.LoggerMock.Object,
                this.CustomerRepositoryMock.Object);

            this.Fixture = new Fixture();
        }

        [Fact]
        public async Task GetAllCustomerReturnsEmptyResult()
        {
            this.CustomerRepositoryMock.Setup(x => x.GetAllCustomers()).ReturnsAsync(new List<CustomerModel>());

            var result = await this.CustomerService.GetAllCustomers();

            Assert.Empty(result);
            this.CustomerRepositoryMock.Verify(x => x.GetAllCustomers(), Times.Once);
        }

        [Fact]
        public async Task GetAllCustomerReturnsResult()
        {
            var customerDataToReturn = this.Fixture.CreateMany<CustomerModel>(5).ToList();
            this.CustomerRepositoryMock.Setup(x => x.GetAllCustomers()).ReturnsAsync(customerDataToReturn);

            var result = await this.CustomerService.GetAllCustomers();

            Assert.Equal(customerDataToReturn.Count, result.Count);
            this.CustomerRepositoryMock.Verify(x => x.GetAllCustomers(), Times.Once);
        }
    }
}
