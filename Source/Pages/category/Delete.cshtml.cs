using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;

namespace RestaurantManagement.Pages.category
{
    public class DeleteModel : PageModel
    {
        private readonly RestaurantManagement.Models.RestaurantManagementContext _context;

        public DeleteModel(RestaurantManagement.Models.RestaurantManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FoodCategory FoodCategory { get; set; } = default!;
        public IList<Food> Food { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.FoodCategories == null)
            {
                return NotFound();
            }

            var foodcategory = await _context.FoodCategories.FirstOrDefaultAsync(m => m.Id == id);

            if (foodcategory == null)
            {
                return NotFound();
            }
            else 
            {
                FoodCategory = foodcategory;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.FoodCategories == null)
            {
                return NotFound();
            }
            var foodcategory = await _context.FoodCategories.FindAsync(id);

            if (foodcategory != null)
            {
                // Kiểm tra xem có dữ liệu Food thuộc Category đó hay không
                var foodInCategory = await _context.Foods.AnyAsync(f => f.CategoryId == id);

                if (foodInCategory)
                {
                    // Hiển thị thông báo lỗi hoặc chuyển hướng đến trang thông báo lỗi
                    TempData["AlertMessage"] = "Delete failed! Food still exists in this Category";
                    return RedirectToPage("./Index");
                }
                else
                {
                    FoodCategory = foodcategory;
                    _context.FoodCategories.Remove(FoodCategory);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
