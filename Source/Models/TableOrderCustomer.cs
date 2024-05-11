using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models
{
    public partial class TableOrderCustomer
    {
        public TableOrderCustomer()
        {
            FoodTables = new HashSet<FoodTable>();
        }

        public int Id { get; set; }
        public int? TableOrderId { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual TableOrder? TableOrder { get; set; }
        public virtual ICollection<FoodTable> FoodTables { get; set; }
    }
}
