using NorthTraderAPI.Models;

namespace NorthTraderAPI.DataServices
{
    public interface ICustomerServices
    {
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerAsync(string customerId);
        Task<Customer?> AddCustomerAsync(Customer customer, CancellationToken cancel);
        Task<Customer?> UpdateCustomerAsync(Customer customer, CancellationToken cancel);
        void DeleteCustomerAsync(string customerId);
    }
}