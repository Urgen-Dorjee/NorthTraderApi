using NorthTraderAPI.Models;

namespace NorthwindTradersTest.DataServices;

public static class TestRepo
{
    public static List<Customer> GetAllCustomers()
    {
       var customers = new List<Customer>()
        {
            new ()
            {
                CustomerId = "ALFKI", Address = "Obere Str. 57", City = "Berlin",
                CompanyName = "Alfreds Futterkiste", ContactName = "Maria Anders",
                ContactTitle = "Sales Representative", Country = "Germany", Fax = "030-0076545",
                Phone = "030-0074321", PostalCode = "12209"
            },
            new()
            {
                CustomerId = "ANATR", Address = "Avda. de la Constitución 2222", City = "México D.F.",
                CompanyName = "Ana Trujillo Emparedados y helados", ContactName = "Ana Trujillo",
                ContactTitle = "Owner", Country = "Mexico", Fax = "(5) 555-3745", Phone = "(5) 555-4729",
                PostalCode = "05021"
            },
            new()
            {
                CustomerId = "ANTON", Address = "Mataderos  2312", City = "México D.F.",
                CompanyName = "Antonio Moreno Taquería", ContactName = "Antonio Moreno", ContactTitle = "Owner",
                Country = "Mexico", Fax = "", Phone = "(5) 555-3932", PostalCode = "05023"
            },
            new()
            {
                CustomerId = "AROUT", Address = "120 Hanover Sq.", City = "London", CompanyName = "Around the Horn",
                ContactName = "Thomas Hardy", ContactTitle = "Sales Representative", Country = "UK",
                Fax = "(171) 555-6750", Phone = "(171) 555-7788", PostalCode = "WA1 1DP"
            },
        };
        return customers;
    }

    public static Customer AddCustomer()
    {
        var customer = new Customer
        {
            CustomerId = "UREAL", 
            Address = "2134 Near Lower TCV", 
            City = "Dharamshala Lower TCV should not exceed fifteen characters",
            CompanyName = "Great Lakes Food Market", 
            ContactName = "Urgen Dorjee", 
            ContactTitle = "Software Manager",
            Country = "USA", 
            Fax = "807-663-4747",
            Phone = "(503) 555-7555", 
            PostalCode = "95348", 
            Region = "CA"
        };
        return customer;
    }
}



