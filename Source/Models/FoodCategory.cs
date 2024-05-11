﻿using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models
{
    public partial class FoodCategory
    {
        public FoodCategory()
        {
            Foods = new HashSet<Food>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Food> Foods { get; set; }
    }
}
