using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagement.Pages.Combo
{
	public class DetailsModel : PageModel
    {
        private readonly RestaurantManagement.Models.RestaurantManagementContext _context;
        public IList<Models.Food> Food { get; set; }
        public DetailsModel(RestaurantManagement.Models.RestaurantManagementContext context)
        {
            _context = context;
        }

      public Models.Combo Combo { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Combos == null)
            {
                return NotFound();
            }

            var combo = await _context.Combos.FirstOrDefaultAsync(m => m.Id == id);
            if (combo == null)
            {
                return NotFound();
            }
            else 
            {
                Combo = combo;
				var foodCombos = _context.FoodCombos.Where(x => x.ComboId == id).ToArray();
                var foods = new List<Models.Food>();
				foreach (var foodCombo in foodCombos)
				{
					foods.Add(_context.Foods.Find(foodCombo.FoodId));
				}
                Food = foods;
			}
            return Page();
        }
    }
}
