﻿using Microsoft.Extensions.Logging;
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
            var viewResult = Assert.IsType<Task<List<Customer>>>(result);
            Assert.Equal(4, viewResult.Result.Count);
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
            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions();
        }

        [Fact]
        public void Add_Customer_Async_Should_Return_No_Content()
        {
            //Arrange
            var customer = TestRepo.AddCustomer();
            _service.Setup(c => c.AddCustomerAsync(customer, CancellationToken.None));
            var controller = new CustomersController(_service.Object, _logger.Object);

            //Act
            var actionResult = controller.AddCustomer(customer, CancellationToken.None);
            var result = actionResult.Result;

            //Assert

            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions();
        }
    }
}
