using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;

namespace RestaurantManagement.Pages.Combo
{
    public class EditModel : PageModel
    {
        private readonly RestaurantManagementContext _context;

        public EditModel(RestaurantManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Combo Combo { get; set; } = default!;
        public IList<Food> Food { get; set; }

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
            Combo = combo;
            var foodCombos = _context.FoodCombos.Where(x => x.ComboId == id).ToArray();
            var foods = new List<Food>();
            foreach (var foodCombo in foodCombos)
            {
                foods.Add(_context.Foods.Find(foodCombo.FoodId));
            }
            Food = foods;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
			_context.FoodCombos.RemoveRange(_context.FoodCombos.Where(foodCombo => foodCombo.ComboId == Combo.Id));
            _context.SaveChanges();

			var foodCombosId = Request.Form["FoodCombos"];
			int? price = 0;
			foreach (var foodComboId in foodCombosId)
			{
				FoodCombo foodCombo = new FoodCombo
				{
					ComboId = Combo.Id,
					FoodId = int.Parse(foodComboId)
				};
				price += _context.Foods.Find(int.Parse(foodComboId)).Price;
				_context.FoodCombos.Add(foodCombo);
			}

			//modify price of added combo
			Combo.Price = price  ;
			_context.Attach(Combo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComboExists(Combo.Id))
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

		public List<Food> GetAllFoods()
		{
			return _context.Foods.ToList();
		}

		private bool ComboExists(int id)
        {
            return (_context.Combos?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
