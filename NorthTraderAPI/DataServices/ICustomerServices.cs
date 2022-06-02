using NorthTraderAPI.Models;

namespace NorthTraderAPI.DataServices
{
    public interface ICustomerServices
    {
        Task<List<Customer>> GetAllCustomers();
    }
}
