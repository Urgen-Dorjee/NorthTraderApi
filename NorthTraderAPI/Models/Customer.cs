using System.ComponentModel.DataAnnotations;

namespace NorthTraderAPI.Models
{
    public class Customer 
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public string? CustomerId { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactName { get; set; }
        public string? ContactTitle { get; set; }
        public string? Address { get; set; }
        [MaxLength(15,ErrorMessage = "City Should not exceed 5 Characters long")]
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }

        public ICollection<Order> Orders { get; private set; }
    }
}
