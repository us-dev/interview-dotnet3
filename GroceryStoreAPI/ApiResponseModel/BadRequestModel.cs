namespace GroceryStoreAPI.ApiResponseModel
{
    /// <summary>
    /// Represents a model to be returned to Client when 4XX status code is to be returned.
    /// </summary>
    public class BadRequestModel 
    {
        /// <summary>
        /// Error Id
        /// </summary>
        /// <example>404</example>
        public string Error { get; set; }

        /// <summary>
        /// String representing the error message.
        /// </summary>
        /// <example>Id doesn't exist.</example>
        public string Message { get; set; }

        /// <summary>
        /// Detailed error description.
        /// </summary>
        /// <example>The id that was provided doesn't link to any of the Customer data. Please check the data and resend the request.</example>
        public string Description { get; set; }
    }
}
