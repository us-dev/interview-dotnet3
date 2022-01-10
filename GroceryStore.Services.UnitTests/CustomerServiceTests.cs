using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
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

        [Fact]
        public async Task GetCustomerByIdReturnsResult()
        {
            var customerDataToReturn = this.Fixture.CreateMany<CustomerModel>(5).ToList();

            // Only return if the Id is the 2nd element.
            this.CustomerRepositoryMock.Setup(x => x.GetCustomerById(customerDataToReturn[1].Id))
                .ReturnsAsync(customerDataToReturn[1]);

            var result = await this.CustomerService.GetCustomerById(customerDataToReturn[1].Id);

            Assert.Equal(customerDataToReturn[1].Name, result.Name);
            Assert.Equal(customerDataToReturn[1].Id, result.Id);

            this.CustomerRepositoryMock.Verify(x => x.GetCustomerById(customerDataToReturn[1].Id), Times.Once);
        }

        [Fact]
        public async Task AddCustomerReturnsResult()
        {
            var customerDataToReturn = this.Fixture.Create<CustomerModel>();
            
            // Resetting the id
            customerDataToReturn.Id = 0;

            // Only return if the Id is the 2nd element.
            this.CustomerRepositoryMock.Setup(x => x.AddCustomer(It.IsAny<CustomerModel>()))
                .ReturnsAsync(customerDataToReturn);

            var result = await this.CustomerService.AddCustomer(customerDataToReturn);

            Assert.Equal(customerDataToReturn.Name, result.Name);

            this.CustomerRepositoryMock.Verify(x => x.AddCustomer(customerDataToReturn), Times.Once);
        }

        [Fact]
        public async Task UpdateCustomerReturnsResult()
        {
            var customerDataToReturn = this.Fixture.Create<CustomerModel>();

            // Only return if the Id is the 2nd element.
            this.CustomerRepositoryMock.Setup(x => x.UpdateCustomer(customerDataToReturn))
                .ReturnsAsync(customerDataToReturn);

            var result = await this.CustomerService.UpdateCustomer(customerDataToReturn);

            Assert.Equal(customerDataToReturn.Name, result.Name);

            this.CustomerRepositoryMock.Verify(x => x.UpdateCustomer(customerDataToReturn), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomerReturnsId()
        {
            var customerDataToReturn = this.Fixture.Create<CustomerModel>();

            // Only return if the Id is the 2nd element.
            this.CustomerRepositoryMock.Setup(x => x.DeleteCustomer(customerDataToReturn.Id))
                .ReturnsAsync(customerDataToReturn.Id);

            var result = await this.CustomerService.DeleteCustomer(customerDataToReturn.Id);

            Assert.Equal(customerDataToReturn.Id, result);

            this.CustomerRepositoryMock.Verify(x => x.DeleteCustomer(customerDataToReturn.Id), Times.Once);
        }
    }
}
