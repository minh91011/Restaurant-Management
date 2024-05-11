using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagement.Pages.Combo
{
	public class DeleteModel : PageModel
    {
        private readonly RestaurantManagement.Models.RestaurantManagementContext _context;

        public DeleteModel(RestaurantManagement.Models.RestaurantManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
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
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Combos == null)
            {
                return NotFound();
            }
            var combo = await _context.Combos.FindAsync(id);

            if (combo != null)
            {
                Combo = combo;
                _context.FoodCombos.RemoveRange(_context.FoodCombos.Where(foodCombo => foodCombo.ComboId == Combo.Id));
                _context.Combos.Remove(Combo);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
