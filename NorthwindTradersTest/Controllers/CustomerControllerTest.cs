using Microsoft.AspNetCore.Mvc;
using Moq;
using NorthTraderAPI.Controllers;
using NorthTraderAPI.DataServices;
using NorthTraderAPI.Models;
using NorthwindTradersTest.DataServices;
using Shouldly;

namespace NorthwindTradersTest.Controllers
{
    public class CustomerControllerTest
    {

        private readonly Mock<ICustomerServices> service;
        public CustomerControllerTest()
        {
            service = new Mock<ICustomerServices>();
        }


        [Fact]
        public void GetAllCustomersAsync()
        {
            //Arrange
            service.Setup(customer => customer.GetAllCustomersAsync())
                .ReturnsAsync(TestRepo.GetAllCustomers);
            var controller = new CustomersController(service.Object);

            //Act
            var result = controller.GetAllCustomersAsync();

            //Assert
            var viewResult = Assert.IsType<Task<List<Customer>>>(result);
            Assert.Equal(4, viewResult.Result.Count);
        }

        [Fact]
        public void Get_CustomerAsync_Should_Return_Customer_By_Id()
        {
            //Arrange 
            var customers = TestRepo.GetAllCustomers();
            var firstCustomer = customers[0];
            service.Setup(c => c.GetCustomerAsync("ALFKI"))
                .ReturnsAsync(firstCustomer);
            var controller = new CustomersController(service.Object);

            //Act
            var actionResult = controller.GetCustomerAsync("ALFKI");
            var result = actionResult.Result;

            //Assert
            result.ShouldNotBeNull();
            result.ShouldNotBeOfType<Task<Customer>>();
        }
    }
}
