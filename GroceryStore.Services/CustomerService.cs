using GroceryStore.Models;
using GroceryStore.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroceryStore.Services
{
    /// <summary>
    /// Service layer class providing actions for Customer domain.
    /// </summary>
    public class CustomerService : ICustomerService
    {
        /// <summary>
        /// Gets or sets an instance of <see cref="ILogger"/>
        /// </summary>
        private ILogger<CustomerService> Logger { get; set; }

        /// <summary>
        /// Gets or sets an instance of <see cref="CustomerRepository"/>
        /// </summary>
        private ICustomerRepository CustomerRepository { get; set; }

        /// <summary>
        /// Default constructor for <see cref="CustomerService"/>
        /// </summary>
        /// <param name="logger">An instance of <see cref="ILogger"/></param>
        /// <param name="customerRepository">An instance of <see cref="ICustomerRepository"/></param>
        public CustomerService(
            ILogger<CustomerService> logger,
            ICustomerRepository customerRepository)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.CustomerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        /// <inheritdoc />
        public async Task<IList<CustomerModel>> GetAllCustomers()
        {
            return await this.CustomerRepository.GetAllCustomers();
        }
        
        /// <inheritdoc />
        public async Task<CustomerModel> GetCustomerById(long id)
        {
            return await this.CustomerRepository.GetCustomerById(id);
        }

        /// <inheritdoc />
        public async Task<CustomerModel> AddCustomer(CustomerModel newCustomerModel)
        {
            return await this.CustomerRepository.AddCustomer(newCustomerModel);
        }

        /// <inheritdoc />
        public async Task<CustomerModel> UpdateCustomer(CustomerModel updatedCustomerModel)
        {
            return await this.CustomerRepository.UpdateCustomer(updatedCustomerModel);
        }

        /// <inheritdoc />
        public async Task<long> DeleteCustomer(long id)
        {
            return await this.CustomerRepository.DeleteCustomer(id);
        }

    }
}
