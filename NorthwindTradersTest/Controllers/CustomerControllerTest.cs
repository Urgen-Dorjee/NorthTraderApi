using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NorthTraderAPI.Controllers;
using NorthTraderAPI.DataServices;
using NorthTraderAPI.Models;
using NorthwindTradersTest.DataServices;
//using Shouldly;
using FluentAssertions;

namespace NorthwindTradersTest.Controllers
{
    public class CustomerControllerTest
    {

        private readonly Mock<ICustomerServices> _service;

        private readonly Mock<ILogger<CustomersController>> _logger;
        public CustomerControllerTest()
        {
            _service = new Mock<ICustomerServices>();
            _logger = new Mock<ILogger<CustomersController>>();
        }


        [Fact]
        public void Get_All_Customers_Async_Should_Return_All()
        {
            //Arrange
            _service.Setup(customer => customer.GetAllCustomersAsync())
                .ReturnsAsync(TestRepo.GetAllCustomers);
            var controller = new CustomersController(_service.Object, _logger.Object);

           
            //Act
            var result = controller.GetAllCustomers();          

            //Assert          
            //var viewResult = Assert.IsType<Task<ActionResult<ICollection<Customer>>>>(result);
            var view = Assert.IsType<Task<ActionResult<List<Customer>>>>(result);
            view.Should().NotBeNull();     
  
        }

        [Fact]
        public void Get_Customer_Async_Should_Return_A_Customer()
        {
            //Arrange 
            var customers = TestRepo.GetAllCustomers();
            var firstCustomer = customers[1];
            _service.Setup(c => c.GetCustomerAsync("ANATR"))
                .ReturnsAsync(firstCustomer);
            var controller = new CustomersController(_service.Object, _logger.Object);

            //Act
            var actionResult = controller.GetCustomer("ANATR");
            var result = actionResult.Result;

            //Assert
            //result.ShouldNotBeNull();
            //result.ShouldSatisfyAllConditions();
            result.Should().NotBeNull();
           
        }

        [Fact]
        public void Add_Customer_Async_Should_Return_Content_Created_201()
        {
            //Arrange
            var customer = TestRepo.AddCustomer();
            _service.Setup(c => c.AddCustomerAsync(customer, CancellationToken.None));
            var controller = new CustomersController(_service.Object, _logger.Object);

            //Act
            var actionResult = controller.AddCustomer(customer, CancellationToken.None);
            var result = actionResult.Result;

            //Assert

            result.Should().NotBeNull();
          
        }

        [Fact]
        public void Delete_Async_Customer_Should_Return_Success()
        {

            //Arrange 
            const string custId = "URGEN";
            _service.Setup(c => c.DeleteCustomerAsync(custId));
            var controller = new CustomersController(_service.Object, _logger.Object);

            //Act
            controller.DeleteCustomer(custId);

            //Assert
            _service.Verify(c => c.DeleteCustomerAsync(custId), Times.Once);

        }
    }
}
