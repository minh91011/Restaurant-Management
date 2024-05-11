using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models
{
    public partial class Customer
    {
        public Customer()
        {
            TableOrderCustomers = new HashSet<TableOrderCustomer>();
        }

        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Fullname { get; set; }
        public int? Phone { get; set; }
        public string? Email { get; set; }

        public virtual ICollection<TableOrderCustomer> TableOrderCustomers { get; set; }
    }
}
