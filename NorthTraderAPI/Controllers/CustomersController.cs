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

      
        public CustomersController(ICustomerServices customerContext)
        {
            _customerContext = customerContext;
           
        }

        [HttpGet]
        public async Task<List<Customer>> GetAllCustomers()
        {
            return await _customerContext.GetAllCustomers();
        }
    }
}
