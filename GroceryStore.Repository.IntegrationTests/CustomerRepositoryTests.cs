using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using GroceryStore.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace GroceryStore.Repository.IntegrationTests
{
    public class CustomerRepositoryTests
    {
        public Mock<ILogger<CustomerRepository>> LoggerMock { get; set; }

        public Mock<IOptions<DataStoreSettings>> DataStoreSettingsMock { get; set; }

        public ICustomerRepository CustomerRepository { get; set; }

        public Fixture Fixture { get; set; }

        public CustomerRepositoryTests()
        {
            this.LoggerMock = new Mock<ILogger<CustomerRepository>>();
            this.DataStoreSettingsMock = new Mock<IOptions<DataStoreSettings>>();

            this.CustomerRepository = new CustomerRepository(
                this.LoggerMock.Object,
                this.DataStoreSettingsMock.Object);

            this.Fixture = new Fixture();

            this.DataStoreSettingsMock.Setup(x => x.Value)
                .Returns(new DataStoreSettings { DataStoreJsonFileName = "database.json" });
        }

        [Fact]
        public async Task CustomerAllCrudOperationsFlow()
        {
            var customerModelToAdd = this.Fixture.Create<CustomerModel>();
            customerModelToAdd.Id = 0; //Reset the Id as if this was a new model.

            // Add the customer to the datastore and ensure that a new Id was created.
            var addedCustomerModel = await this.CustomerRepository.AddCustomer(customerModelToAdd);
            Assert.NotNull(addedCustomerModel);
            Assert.NotEqual(0, addedCustomerModel.Id);
            Assert.Equal(customerModelToAdd.Name, addedCustomerModel.Name);

            // Read all the customers and assert that the list is not empty and that the newly added customer exists.
            var readAllCustomerModels = await this.CustomerRepository.GetAllCustomers();
            Assert.NotEmpty(readAllCustomerModels);
            Assert.NotNull(readAllCustomerModels.FirstOrDefault(x => x.Id == addedCustomerModel.Id));

            // Read the specific customer by id
            var readCustomer = await this.CustomerRepository.GetCustomerById(customerModelToAdd.Id);
            Assert.NotNull(readCustomer);
            Assert.Equal(customerModelToAdd.Id, readCustomer.Id);

            // Update the customer data and save it
            readCustomer.Name = "Test Name";
            var updatedCustomer = await this.CustomerRepository.UpdateCustomer(readCustomer);
            Assert.NotNull(updatedCustomer);
            Assert.Equal("Test Name", updatedCustomer.Name);

            // Read the same customer again and make sure the new Get returns the same data
            var readUpdatedCustomerModel = await this.CustomerRepository.GetCustomerById(customerModelToAdd.Id);
            Assert.NotNull(readUpdatedCustomerModel);
            Assert.Equal(readUpdatedCustomerModel.Id, readUpdatedCustomerModel.Id);
            Assert.Equal("Test Name", readUpdatedCustomerModel.Name);

            // Finally delete the customer
            var deletedCustomerId = await this.CustomerRepository.DeleteCustomer(readUpdatedCustomerModel.Id);
            Assert.Equal(readUpdatedCustomerModel.Id, deletedCustomerId);

            // Try read the deleted Customer by Id. It should return null
            var readDeletedCustomerModel = await this.CustomerRepository.GetCustomerById(readUpdatedCustomerModel.Id);
            Assert.Null(readDeletedCustomerModel);

            // Try reading all the customers. The deleted Id should not be there
            var readAllCustomerModelsAfterDeletion = await this.CustomerRepository.GetAllCustomers();
            Assert.Null(readAllCustomerModelsAfterDeletion.FirstOrDefault(x => x.Id == readUpdatedCustomerModel.Id));
        }
    }
}
