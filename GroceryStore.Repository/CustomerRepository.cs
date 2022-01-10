using GroceryStore.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace GroceryStore.Repository
{
    /// <summary>
    /// Repository class providing custom logic for Customer domain object.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        /// <summary>
        /// Gets or sets the ILogger instance.
        /// </summary>
        private ILogger<CustomerRepository> Logger { get; set; }

        /// <summary>
        /// Gets or sets the Datastore settings using IOptions.
        /// </summary>
        private IOptions<DataStoreSettings> DataStoreSettings { get; set; }

        /// <summary>
        /// Constructor for the <see cref="CustomerRepository"/>
        /// </summary>
        /// <param name="logger">An instance of <see cref="ILogger"/></param>
        /// <param name="dataStoreSettings">An instance of <see cref="DataStoreSettings"/>.</param>
        public CustomerRepository(
            ILogger<CustomerRepository> logger,
            IOptions<DataStoreSettings> dataStoreSettings)
        {
            this.Logger = logger;
            this.DataStoreSettings = dataStoreSettings;
        }

        /// <summary>
        /// Gets all the customer in the system.
        /// </summary>
        /// <returns>A list of <see cref="CustomerModel"/></returns>
        public async Task<IList<CustomerModel>> GetAllCustomers()
        {
            var groceryStoreModel = await this.ReadFile();
            return groceryStoreModel.Customers;
        }

        /// <summary>
        /// Gets the particular CustomerModel by Id.
        /// </summary>
        /// <param name="id">An Id representing a Customer Model.</param>
        /// <returns>Existing <see cref="CustomerModel"/> object.</returns>
        public async Task<CustomerModel> GetCustomerById(long id)
        {
            var groceryStoreModel = await this.ReadFile();
            return groceryStoreModel.Customers.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Adds/Creates a new customer in the datastore.
        /// </summary>
        /// <param name="newCustomerModel">A new <see cref="CustomerModel"/> to be added.</param>
        /// <returns>The newly added customer model.</returns>
        public async Task<CustomerModel> AddCustomer(CustomerModel newCustomerModel)
        {
            var fileContent = await this.ReadFile();
            var allCustomers = fileContent.Customers;
            
            // create and assign the new id
            var newCustomerId = allCustomers.Last().Id + 1;
            newCustomerModel.Id = newCustomerId;

            // Add the new customer to the bottom of the list.
            allCustomers.Add(newCustomerModel);

            await this.SaveFile(fileContent);
            return newCustomerModel;
        }

        /// <summary>
        /// Updates the Customer information in the existing datastore.
        /// </summary>
        /// <param name="updatedCustomerModel">A <see cref="CustomerModel"/></param>
        /// <returns>The Customer object that has been successfully updated.</returns>
        public async Task<CustomerModel> UpdateCustomer(CustomerModel updatedCustomerModel)
        {
            var fileContent = await this.ReadFile();
            var allCustomers = fileContent.Customers;
            var existingCustomerToUpdate = allCustomers.FirstOrDefault(x => x.Id == updatedCustomerModel.Id);

            // Update the information to the existing model
            existingCustomerToUpdate.Name = updatedCustomerModel.Name;

            // Save the file
            await this.SaveFile(fileContent);

            return existingCustomerToUpdate;
        }

        /// <summary>
        /// Orchestrates the deletion of a Customer object from the datastore.
        /// </summary>
        /// <param name="customerId">An id representing a Customer <see cref="CustomerModel"/></param>
        /// <returns>An id of the deleted record.</returns>
        public async Task<long> DeleteCustomer(long customerId)
        {
            var fileContent = await this.ReadFile();
            var allCustomers = fileContent.Customers;
            var customerToRemove = allCustomers.FirstOrDefault(x => x.Id == customerId);
            
            // Check if the customer exists in the system.
            if (customerToRemove != null)
            {
                allCustomers.Remove(customerToRemove);
                await this.SaveFile(fileContent);
            }
            else
            {
                // if customer doesn't exist in the system then reset the id to 0.
                customerId = 0;
            }

            return customerId;
        }

        /// <summary>
        /// Reads the file and returns the content as a <see cref="GroceryStoreModel"/> object.
        /// </summary>
        /// <returns>A populated <see cref="GroceryStoreModel"/></returns>
        private async Task<GroceryStoreModel> ReadFile()
        {
            GroceryStoreModel groceryStoreEntity;

            try
            {
                var fileContent = await File.ReadAllTextAsync(this.DataStoreSettings.Value.DataStoreJsonFileName);
                groceryStoreEntity = JsonConvert.DeserializeObject<GroceryStoreModel>(fileContent);
            }
            catch (Exception ex)
            {
                this.Logger.LogCritical(ex, "Unable to read from JSON file.");
                throw;
            }

            return groceryStoreEntity;
        }

        /// <summary>
        /// Saves the file by writing a new content.
        /// </summary>
        /// <param name="groceryStoreModelToSave"></param>
        /// <returns>An empty Task.</returns>
        private async Task SaveFile(GroceryStoreModel groceryStoreModelToSave)
        {
            try
            {
                await File.WriteAllTextAsync(
                     this.DataStoreSettings.Value.DataStoreJsonFileName,
                     JsonConvert.SerializeObject(groceryStoreModelToSave));
            }
            catch (Exception ex)
            {
                this.Logger.LogCritical(ex, "Unable to write to the JSON file.");
                throw;
            }
        }
    }
}
