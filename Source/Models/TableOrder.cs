using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models
{
    public partial class TableOrder
    {
        public TableOrder()
        {
            TableOrderCustomers = new HashSet<TableOrderCustomer>();
        }

        public int Id { get; set; }
        public string? Description { get; set; }
        public int? Floor { get; set; }
        public int Number { get; set; }

        public virtual ICollection<TableOrderCustomer> TableOrderCustomers { get; set; }
    }
}
