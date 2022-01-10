using System.Collections.Generic;

namespace GroceryStore.Models
{
    /// <summary>
    /// Model representing the Grocery Store Model.
    /// </summary>
    public class GroceryStoreModel
    {
        /// <summary>
        /// Represents a list of all the Customer in the GroceryStore application.
        /// </summary>
        public IList<CustomerModel> Customers { get; set; }
    }
}
