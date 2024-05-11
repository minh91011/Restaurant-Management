using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;

namespace RestaurantManagement.Pages.Order
{
    public class IndexModel : PageModel
    {
        private readonly IHubContext<ResHub.ResHub> _hubContext;
        private readonly RestaurantManagementContext _context;
        public IndexModel(RestaurantManagementContext context, IHubContext<ResHub.ResHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public List<Models.Food> food = new List<Food> { };
        public List<Models.FoodCategory> foodCategories = new List<FoodCategory> { };
        public List<Models.Combo> combo = new List<Models.Combo> { };
        public List<Models.FoodCombo> foodcombo = new List<FoodCombo> { };
        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                // User is authenticated, you can redirect to a secured page
                return RedirectToPage("/SecurePage");
            }
            else
            {
                foodCategories = _context.FoodCategories.Include("Foods").ToList();
                combo = _context.Combos.Include("FoodCombos").ToList();
                foodcombo = _context.FoodCombos.Include("Food").ToList();
                return Page();
            }

          
        }
        public IActionResult OnPost(String[] foodId, String[] comboId , int[] number_food, int[] number_combo)
        {
            if(foodId.Length!= 0)
            {
                for(int i = 0; i < foodId.Length; i++)
                {
                    _context.FoodTables.Add(new Models.FoodTable
                    {
                        FoodId = int.Parse(foodId[i]),
                        TableOrderCustomerId = int.Parse(HttpContext.Session.GetString("TableOrderCustomerId")) ,// get from session
                        Number = number_food[i],
                    }); 
                    _context.SaveChanges();

                }
                 
            }
            if(comboId.Length!= 0)
            {
                for (int i = 0;i < comboId.Length; i++)
                {
                    _context.FoodTables.Add(new Models.FoodTable
                    {
                        ComboId = int.Parse(comboId[i]),
                        TableOrderCustomerId =
                        int.Parse(HttpContext.Session.GetString("TableOrderCustomerId")) ,// get from session,
                        Number = number_combo[i] 
                    });
                    _context.SaveChanges();

                }

                
            }
         
            return RedirectToPage("/order/listorderdetail");
        }
    }
}
