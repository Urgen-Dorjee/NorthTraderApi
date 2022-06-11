using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NorthTraderAPI.DataServices;
using NorthTraderAPI.Models;

namespace NorthTraderAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
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
        /// Display a list of all Customers
        /// </summary>
        /// <returns>It returns all the customer information from the database</returns>
        [HttpGet]
        public async Task<List<Customer>> GetAllCustomers()
        {
            var customers= await _customerContext.GetAllCustomersAsync();
            return customers;
        }

        /// <summary>
        /// Retrieving a Customer with specified ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>It returns a customer info from database</returns>
        /// <exception cref="KeyNotFoundException"></exception>

        [HttpGet("{id}")]
        public async Task<IActionResult?> GetCustomer(string id)
        {
            _logger.LogInformation($"Retrieving Customer Info of ID: {id}");
            var customer = await _customerContext.GetCustomerAsync(id);
            if (customer is null)
            {
                throw new KeyNotFoundException($"Did not find a user of ID: {id}");
            }
            return Ok(customer);

        }
        /// <summary>
        /// Adding a Customer to a database
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="cancel"></param>
        /// <returns>A newly created customer data</returns>
        [HttpPost]
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
        /// Updating the customer records
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="customer"></param>
        /// <param name="cancel"></param>
        /// <returns>Returns Updated customer records</returns>

        [HttpPut("{customerId}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCustomer(string customerId, [FromBody]Customer customer, CancellationToken cancel)
        {
            var response = await _customerContext.GetCustomerAsync(customerId);
            if (response!.CustomerId != customer.CustomerId) return BadRequest();
            await _customerContext.UpdateCustomerAsync(customer, cancel);
            return Ok(customer);
        }
    }
}
