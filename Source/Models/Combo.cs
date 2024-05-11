using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models
{
    public partial class Combo
    {
        public Combo()
        {
            FoodCombos = new HashSet<FoodCombo>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }

        public virtual ICollection<FoodCombo> FoodCombos { get; set; }
    }
}
