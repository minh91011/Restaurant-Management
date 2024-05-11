using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models
{
    public partial class FoodCombo
    {
        public int Id { get; set; }
        public int? FoodId { get; set; }
        public int ComboId { get; set; }

        public virtual Combo Combo { get; set; } = null!;
        public virtual Food? Food { get; set; }
    }
}
