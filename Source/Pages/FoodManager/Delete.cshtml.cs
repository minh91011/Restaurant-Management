using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;

namespace RestaurantManagement.Pages.FoodManager
{
    public class DeleteModel : PageModel
    {
        private readonly RestaurantManagement.Models.RestaurantManagementContext _context;

        public DeleteModel(RestaurantManagement.Models.RestaurantManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Food Food { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Foods == null)
            {
                return NotFound();
            }

            var food = await _context.Foods.Include(f => f.Category).FirstOrDefaultAsync(m => m.Id == id);

            if (food == null)
            {
                return NotFound();
            }
            else 
            {
                Food = food;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Foods == null)
            {
                return NotFound();
            }
            var food = await _context.Foods.FindAsync(id);

            if (food != null)
            {
                List<FoodCombo> foodCombo =  _context.FoodCombos.Where(m => m.FoodId == id).ToList();
                foodCombo.ForEach(fb =>
                {
                    _context.FoodCombos.Remove(fb);
                    _context.SaveChanges();
                });
                List<FoodTable> foodTables = _context.FoodTables.Where(m => m.FoodId == id).ToList();
                foodTables.ForEach(ft =>
                {
                    _context.FoodTables.Remove(ft);
                    _context.SaveChanges();
                });



                Food = food;
                _context.Foods.Remove(Food);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
