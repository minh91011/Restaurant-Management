using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantManagement.Models;

namespace RestaurantManagement.Pages.FoodManager
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
            ViewData["Name"] = new SelectList(_context.FoodCategories.Select(fc => fc.Name).Distinct());
            return Page();
        }

        [BindProperty]
        public Food Food { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(IFormFile image)
        {
            if (!ModelState.IsValid || Food == null || _context.Foods == null || _context.FoodCategories == null)
            {
                return Page();
            }

            // Kiểm tra xem Category có tồn tại trong danh sách Categories hay không
            var categoryName = Food.Category.Name; // Giả sử tên Category được nhập từ người dùng
            var existingCategory = _context.FoodCategories.FirstOrDefault(c => c.Name == categoryName);

            if (existingCategory == null)
            {
                // Nếu không tồn tại, tạo mới Category
                existingCategory = new FoodCategory { Name = categoryName };
                _context.FoodCategories.Add(existingCategory);
            }

            // Gán Category cho Food
            Food.Category = existingCategory;

            if (image != null && image.Length > 0)
            {
                var fileName = Path.GetFileName(image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Image", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                Food.Image = $"/Image/{fileName}";
            }

            _context.Foods.Add(Food);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
