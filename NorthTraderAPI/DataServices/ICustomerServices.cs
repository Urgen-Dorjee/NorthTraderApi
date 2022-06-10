using NorthTraderAPI.Models;

namespace NorthTraderAPI.DataServices
{
    public interface ICustomerServices
    {
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerAsync(string customerId);
        Task<Customer?> AddCustomer(Customer customer, CancellationToken cancel);
    }
}