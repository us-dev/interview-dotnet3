using System;

namespace GroceryStore.Models
{
    /// <summary>
    /// Model representing a Customer in the GroceryStore domain.
    /// </summary>
    public class CustomerModel
    {
        /// <summary>
        /// Represents the Id of the Customer.
        /// </summary>
        /// <example>5</example>
        public long Id { get; set; }

        /// <summary>
        /// Indicates the name of the Customer.
        /// </summary>
        /// <example>John Doe</example>
        public string Name { get; set; }
    }
}
