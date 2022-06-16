using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NorthTraderAPI.Controllers;
using NorthTraderAPI.DataServices;
using NorthTraderAPI.Models;
using NorthwindTradersTest.DataServices;
//using Shouldly;
using FluentAssertions;

namespace NorthwindTradersTest.Controllers;

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
        var actionResult = controller.CreateCustomer(customer, CancellationToken.None);
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
    [Fact]
    public void Update_Customer_Returns_Ok()
    {
        // Arrange
        var cust = new Customer()
        {
            CustomerId = "ABCD",
            ContactName = "URGEN DORJEE NEW",
            City = "Lower TCV School Road",
            Country = "INDIA"
        };
        _service.Setup(repo => repo.UpdateCustomerAsync(It.IsAny<Customer>(), CancellationToken.None)).ReturnsAsync(cust);
        var controller = new CustomersController(_service.Object, _logger.Object);

        // Act
        var result = controller.UpdateCustomer(cust.CustomerId, cust, CancellationToken.None);

        // Assert
         var customer = Assert.IsType<Task<ActionResult<Customer>>>(result);
         //Assert.Equal("ABCD", reservation.Should().Equals(cust.CustomerId);
        //Assert.Equal("URGEN DORJEE NEW", reservation.ContactName);
        //Assert.Equal("Lower TCV School Road", reservation.City);
        //Assert.Equal("INDIA", reservation.Country);
    }
}
