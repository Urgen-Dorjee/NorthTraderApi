using Microsoft.AspNetCore.Mvc;
using NorthTraderAPI.DataServices;
using NorthTraderAPI.Models;

namespace NorthTraderAPI.Controllers
{

    [Route("[controller]")]
    [ApiController]  
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<Customer>>> GetAllCustomers()
        {
            return await Task.FromResult(Ok(_customerContext.GetAllCustomersAsync()));
        }

        /// <summary>
        /// { Retrieving a Customer record by customerId }
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>It returns a customer info from database</returns>
        /// <exception cref="KeyNotFoundException"></exception>

        [HttpGet("/customers/{customerId}", Name =nameof(GetCustomer))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult?> GetCustomer(string customerId)
        {
            if (customerId is null) return BadRequest();
            _logger.LogInformation($"Retrieving a Customer Info of CustomerID: {customerId}");
            var customer = await _customerContext.GetCustomerAsync(customerId);
            if (customer is null)
            {
                _logger.LogError("There is no such record exists of {CustomerID}", customerId);
                throw new KeyNotFoundException($"Did not find a any record of CustomerID: {customerId}");
               
            }
            return Ok(customer);
        }
        /// <summary>
        /// { Adding a Customer to a database }
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="cancel"></param>
        /// <returns>A newly created customer data</returns>
        [HttpPost("/customers", Name =nameof(CreateCustomer))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateCustomer([FromBody] Customer customer, CancellationToken cancel)
        {
            if (!ModelState.IsValid) return BadRequest();
            _logger.LogInformation("Adding a Customer in the database");
             return await  Task.FromResult(Ok(_customerContext.AddCustomerAsync(customer, cancel)));
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
        public async Task<ActionResult<Customer>> UpdateCustomer([FromRoute]string customerId, [FromBody] Customer customer, CancellationToken cancel)
        {
            var response = await _customerContext.GetCustomerAsync(customerId);
            if (response!.CustomerId != customer.CustomerId) return BadRequest();
            return await Task.FromResult(Ok(_customerContext.UpdateCustomerAsync(customer, cancel)));
            
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
