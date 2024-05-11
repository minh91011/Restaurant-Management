using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantManagement.Models;

namespace RestaurantManagement.Pages.category
{
    public class CreateModel : PageModel
    {
        private readonly RestaurantManagement.Models.RestaurantManagementContext _context;

        public CreateModel(RestaurantManagement.Models.RestaurantManagementContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public FoodCategory FoodCategory { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.FoodCategories == null || FoodCategory == null)
            {
                return Page();
            }

            _context.FoodCategories.Add(FoodCategory);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
