using Microsoft.AspNetCore.Mvc;
using NorthTraderAPI.DataServices;
using NorthTraderAPI.Models;

namespace NorthTraderAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerServices _customerContext;
        private readonly ILogger<CustomersController> _logger;


        public CustomersController(ICustomerServices customerContext, ILogger<CustomersController> logger)
        {
            _customerContext = customerContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _customerContext.GetAllCustomersAsync();
        }

        [HttpGet("{id}")]
        public async Task<Customer?> GetCustomerAsync(string id)
        {
            return await _customerContext.GetCustomerAsync(id);
        }

        [HttpPost]
        public async Task<Customer?> AddCustomer([FromBody] Customer customer, CancellationToken cancel)
        {
            if (!ModelState.IsValid) return customer;
            _logger.LogInformation("Adding a Customer in the database");
            await _customerContext.AddCustomer(customer, cancel);
            return customer;
        }
    }
}
