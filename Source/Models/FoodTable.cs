using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models
{
    public partial class FoodTable
    {
        public int Id { get; set; }
        public int? FoodId { get; set; }
        public int? TableOrderCustomerId { get; set; }
        public int ComboId { get; set; }
        public int? Number {  get; set; }
        public virtual Food? Food { get; set; }
        public virtual TableOrderCustomer? TableOrderCustomer { get; set; }
    }
}
