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
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _customerContext.GetAllCustomersAsync();
        }

        /// <summary>
        /// Retrieving a Customer with specified ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>It returns a customer info from database</returns>
        /// <exception cref="KeyNotFoundException"></exception>

        [HttpGet("{id}")]
        public async Task<Customer?> GetCustomerAsync(string id)
        {
            _logger.LogInformation($"Retrieving Customer Info of ID: {id}");
            var customer = await _customerContext.GetCustomerAsync(id);
            if (customer is null)
            {
                throw new KeyNotFoundException($"Did not find a user of ID: {id}");
            }
            return customer;

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
        public async Task<Customer?> AddCustomer([FromBody] Customer customer, CancellationToken cancel)
        {
            if (!ModelState.IsValid) return customer;
            _logger.LogInformation("Adding a Customer in the database");
            await _customerContext.AddCustomer(customer, cancel);
            return customer;
        }
    }
}
