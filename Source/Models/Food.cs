using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models
{
    public partial class Food
    {
        public Food()
        {
            FoodCombos = new HashSet<FoodCombo>();
            FoodTables = new HashSet<FoodTable>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public int? CategoryId { get; set; }
        public string? Image { get; set; }

        public virtual FoodCategory? Category { get; set; }
        public virtual ICollection<FoodCombo> FoodCombos { get; set; }
        public virtual ICollection<FoodTable> FoodTables { get; set; }
    }
}
