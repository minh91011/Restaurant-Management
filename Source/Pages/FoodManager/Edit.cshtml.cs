using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;

namespace RestaurantManagement.Pages.FoodManager
{
    public class EditModel : PageModel
    {
        private readonly RestaurantManagement.Models.RestaurantManagementContext _context;

        public EditModel(RestaurantManagement.Models.RestaurantManagementContext context)
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

            var food =  await _context.Foods.FirstOrDefaultAsync(m => m.Id == id);
            if (food == null)
            {
                return NotFound();
            }
            Food = food;
            ViewData["Img"] = Food.Image;
            ViewData["Name"] = new SelectList(_context.FoodCategories.Select(fc => fc.Name).Distinct());
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra xem Category mới có tồn tại trong danh sách Categories hay không
            var newCategoryName = Food.Category.Name; // Giả sử tên Category mới được nhập từ người dùng
            var existingCategory = _context.FoodCategories.FirstOrDefault(c => c.Name == newCategoryName);

            if (existingCategory == null)
            {
                // Nếu không tồn tại, tạo mới Category
                existingCategory = new FoodCategory { Name = newCategoryName };
                _context.FoodCategories.Add(existingCategory);
            }

            // Gán Category mới cho Food
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
            // Cập nhật Food
            _context.Attach(Food).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodExists(Food.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FoodExists(int id)
        {
          return (_context.Foods?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
