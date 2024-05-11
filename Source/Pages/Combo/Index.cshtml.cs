using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagement.Pages.Combo
{
	public class IndexModel : PageModel
	{
		private readonly RestaurantManagement.Models.RestaurantManagementContext _context;

		public IndexModel(RestaurantManagement.Models.RestaurantManagementContext context)
		{
			_context = context;
		}

		public IList<Models.Combo> Combo { get; set; } = default!;

		public async Task OnGetAsync()
		{
			if (_context.Combos != null)
			{
				Combo = await _context.Combos.ToListAsync();
			}
		}
		public List<string> GetFoodsName(int id)
		{
			List<string> foods = new List<string>();
			var foodCombos = _context.FoodCombos.Where(x => x.ComboId == id).ToArray();
			foreach (var food in foodCombos)
			{
				foods.Add(_context.Foods.Find(food.FoodId).Name);
			}
			return foods;
		}
	}

}

