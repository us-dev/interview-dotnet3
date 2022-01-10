using GroceryStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroceryStore.Services
{
    /// <summary>
    /// Interface that defines the actions that can be taken against a Customer.
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Gets all the customer in the system.
        /// </summary>
        /// <returns>A list of <see cref="CustomerModel"/></returns>
        Task<IList<CustomerModel>> GetAllCustomers();

        /// <summary>
        /// Gets the particular CustomerModel by Id.
        /// </summary>
        /// <param name="id">An Id representing a Customer Model.</param>
        /// <returns>Existing <see cref="CustomerModel"/> object.</returns>
        Task<CustomerModel> GetCustomerById(long id);

        /// <summary>
        /// Adds/Creates a new customer in the datastore.
        /// </summary>
        /// <param name="newCustomerModel">A new <see cref="CustomerModel"/> to be added.</param>
        /// <returns>The newly added customer model <see cref="CustomerModel"/>.</returns>
        Task<CustomerModel> AddCustomer(CustomerModel newCustomerModel);

        /// <summary>
        /// Updates the Customer information in the existing datastore.
        /// </summary>
        /// <param name="updatedCustomerModel">A <see cref="CustomerModel"/></param>
        /// <returns>The Customer <see cref="CustomerModel"/> object that has been successfully updated.</returns>
        Task<CustomerModel> UpdateCustomer(CustomerModel updatedCustomerModel);

        /// <summary>
        /// Orchestrates the deletion of a Customer object from the datastore.
        /// </summary>
        /// <param name="customerId">An id representing a Customer <see cref="CustomerModel"/></param>
        /// <returns>An id of the deleted record.</returns>
        Task<long> DeleteCustomer(long customerId);
    }
}