using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthTraderAPI.Models;
public class Shipper
{
    public Shipper()
    {
        Orders = new HashSet<Order>();
    }

    public int ShipperId { get; set; }
    public string? CompanyName { get; set; }
    public string? Phone { get; set; }

    public ICollection<Order> Orders { get; private set; }
}
