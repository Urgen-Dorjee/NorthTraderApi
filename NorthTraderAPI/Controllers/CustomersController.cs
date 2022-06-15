using Microsoft.AspNetCore.Mvc;
using NorthTraderAPI.DataServices;
using NorthTraderAPI.Models;

namespace NorthTraderAPI.Controllers
{

    [Route("[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [Produces("application/json")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerServices _customerContext;
        private readonly ILogger<CustomersController> _logger;


        public CustomersController(ICustomerServices customerContext, ILogger<CustomersController> logger)
        {
            _customerContext = customerContext;
            _logger = logger;
        }

        /// <summary>
        /// { Display a list of all Customers }
        /// </summary>
        /// <returns>It returns all the customer information from the database</returns>
        [HttpGet("/customers", Name = nameof(GetAllCustomers))]
        public async Task<List<Customer>> GetAllCustomers()
        {
            return await _customerContext.GetAllCustomersAsync();
        }

        /// <summary>
        /// { Retrieving a Customer record by customerId }
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>It returns a customer info from database</returns>
        /// <exception cref="KeyNotFoundException"></exception>

        [HttpGet("/customers/{customerId}", Name =nameof(GetCustomer))]
        public async Task<IActionResult?> GetCustomer(string customerId)
        {
            _logger.LogInformation($"Retrieving Customer Info of ID: {customerId}");
            var customer = await _customerContext.GetCustomerAsync(customerId);
            if (customer is null)
            {
                throw new KeyNotFoundException($"Did not find a user of ID: {customerId}");
            }
            return Ok(customer);

        }
        /// <summary>
        /// { Adding a Customer to a database }
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="cancel"></param>
        /// <returns>A newly created customer data</returns>
        [HttpPost("/customers", Name =nameof(AddCustomer))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCustomer([FromBody] Customer customer, CancellationToken cancel)
        {
            if (!ModelState.IsValid) return BadRequest();
            _logger.LogInformation("Adding a Customer in the database");
            await _customerContext.AddCustomerAsync(customer, cancel);
            return Ok(customer);
        }
        /// <summary>
        /// { Updating the customer records }
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="customer"></param>
        /// <param name="cancel"></param>
        /// <returns>Returns Updated customer records</returns>

        [HttpPut("/customers/{customerId}/updateCustomer", Name=nameof(UpdateCustomer))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCustomer([FromRoute]string customerId, [FromBody] Customer customer, CancellationToken cancel)
        {
            var response = await _customerContext.GetCustomerAsync(customerId);
            if (response!.CustomerId != customer.CustomerId) return BadRequest();
            await _customerContext.UpdateCustomerAsync(customer, cancel);
            return Ok(customer);
        }
        /// <summary>
        /// { Delete a customer record from the database }
        /// </summary>
        /// <param name="customerId"></param>

        [HttpDelete("/customers", Name = nameof(DeleteCustomer))]
        public void DeleteCustomer(string customerId)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation($"Deleting customer record of ID: {customerId}");
                _customerContext.DeleteCustomerAsync(customerId);
            }
        }
    }
}
