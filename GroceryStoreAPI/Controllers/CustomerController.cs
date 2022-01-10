using GroceryStore.Models;
using GroceryStore.Services;
using GroceryStoreAPI.ApiResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using GroceryStoreAPI.ApiRequestModel;

namespace GroceryStoreAPI.Controllers
{
    /// <summary>
    /// Allows basic actions related to a customer domain.
    /// </summary>
    [ApiController]
    [Route("customer")]
    [Produces("application/json")]
    public class CustomerController : Controller
    {
        /// <summary>
        /// Gets or sets an instance of Microsoft Logger sink for type <see cref="CustomerController"/>
        /// </summary>
        private ILogger<CustomerController> Logger { get; set; }

        /// <summary>
        /// An instance of <see cref="CustomerService"/> that provides actions to modify Customer entity.
        /// </summary>
        private ICustomerService CustomerService { get; set; }

        /// <summary>
        /// Gets or sets the instance of an Automapper.
        /// </summary>
        public IMapper Mapper { get; set; }

        /// <summary>
        /// Default constructor for <see cref="CustomerController"/>
        /// </summary>
        /// <param name="logger">An instance of Logger. <see cref="Logger"/></param>
        /// <param name="customerService">An instance of <see cref="CustomerService"/></param>
        /// <param name="mapper">An instance of an <see cref="IMapper"/></param>
        public CustomerController(
            ILogger<CustomerController> logger,
            ICustomerService customerService,
            IMapper mapper)
        {
            this.Logger = logger;
            this.CustomerService = customerService;
            this.Mapper = mapper;
        }

        /// <summary>
        /// Provides a list of all customer in the system.
        /// </summary>
        /// <returns>A list of Customer models <see cref="IList{CustomerModel}"/>.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IList<CustomerModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IList<CustomerModel>> GetAllCustomer()
        {
            return await this.CustomerService.GetAllCustomers();
        }

        /// <summary>
        /// Provides a customer object represented by an Id.
        /// </summary>
        /// <param name="customerId" example="1">An Id of existing customer.</param>
        /// <returns>A Customer model <see cref="CustomerModel"/>.</returns>
        [HttpGet("{customerId}")]
        [ProducesResponseType(typeof(CustomerModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CustomerModel>> GetCustomerById(long customerId)
        {
            var customer = await this.CustomerService.GetCustomerById(customerId);
            ActionResult result;

            if (customer != null)
            {
                result = Ok(customer);
            }
            else
            {
                result = NotFound(new BadRequestModel
                {
                    Error = HttpStatusCode.NotFound.ToString(),
                    Message = "Customer information for given ID doesn't exist.",
                });
            }

            return result;
        }

        /// <summary>
        /// Creates a new customer in the system based on the information supplied.
        /// </summary>
        /// <param name="newCustomer">Information on a new customer.</param>
        /// <returns>The recently created <see cref="CustomerModel"/> object</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CustomerModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CustomerModel>> CreateCustomer(CustomerRequestModel newCustomer)
        {
            var customerToBeCreated = this.Mapper.Map<CustomerRequestModel, CustomerModel>(newCustomer);
            var customer = await this.CustomerService.AddCustomer(customerToBeCreated);
            ActionResult result;

            if (customer != null)
            {
                result = StatusCode(201, customer);
            }
            else
            {
                result = StatusCode(500, new BadRequestModel
                {
                    Error = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Customer cannot be created at this time.",
                });
            }

            return result;
        }

        /// <summary>
        /// Updates the information of a customer as represented by <see cref="CustomerModel"/>
        /// </summary>
        /// <param name="existingCustomerModel">An existing Customer <see cref="CustomerModel"/></param>
        /// <returns>Just updated <see cref="CustomerModel"/></returns>
        [HttpPut]
        [ProducesResponseType(typeof(CustomerModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CustomerModel>> UpdateCustomer(CustomerModel existingCustomerModel)
        {
            var customer = await this.CustomerService.UpdateCustomer(existingCustomerModel);
            ActionResult result;

            if (customer != null)
            {
                result = StatusCode(200, customer);
            }
            else
            {
                result = StatusCode(500, new BadRequestModel
                {
                    Error = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Customer cannot be updated at this time.",
                });
            }

            return result;
        }

        /// <summary>
        /// Deletes a customer based on an Id.
        /// </summary>
        /// <param name="customerId" example="4">An id of the customer <seealso cref="CustomerModel"/></param>
        /// <returns>An empty body if delete is successful.</returns>
        [HttpDelete("{customerId}")]
        [ProducesResponseType(typeof(CustomerModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteCustomer(long customerId)
        {
            var deletedCustomerId = await this.CustomerService.DeleteCustomer(customerId);
            ActionResult result;

            if (deletedCustomerId == customerId)
            {
                result = StatusCode(200);
            }
            else
            {
                result = StatusCode(500, new BadRequestModel
                {
                    Error = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Customer cannot be deleted at this time.",
                });
            }

            return result;
        }
    }
}
