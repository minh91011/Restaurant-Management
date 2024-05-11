using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;

namespace RestaurantManagement.Pages.Combo
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
        public Models.Combo Combo { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Combos == null || Combo == null)
            {
                return Page();
            }
            _context.Combos.Add(Combo);
            _context.SaveChanges();
            var latestCombo = _context.Combos.OrderByDescending(combo => combo.Id).FirstOrDefault();

            //adding food combo
            var foodCombosId = Request.Form["FoodCombos"];
            int? price = 0;
            foreach(var foodComboId in foodCombosId)
            {
                FoodCombo foodCombo = new FoodCombo
                {
                    ComboId = latestCombo.Id,
                    FoodId = int.Parse(foodComboId)
                };
                price += _context.Foods.Find(int.Parse(foodComboId)).Price;
                _context.FoodCombos.Add(foodCombo);
            }

            //modify price of added combo
            latestCombo.Price = price ;
            _context.Attach(latestCombo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        public List<Food> GetAllFoods()
        {
            return _context.Foods.ToList();
        }
    }
}
