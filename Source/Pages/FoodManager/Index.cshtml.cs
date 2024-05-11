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
    public class IndexModel : PageModel
    {
        private readonly RestaurantManagement.Models.RestaurantManagementContext _context;

        public IndexModel(RestaurantManagement.Models.RestaurantManagementContext context)
        {
            _context = context;
        }

        public IList<Food> Food { get;set; } = default!;

        public IActionResult  OnGet()
        {

            if (HttpContext.Session.GetString("IsAdmin") == "true")
            {
                Food =   _context.Foods
               .Include(f => f.Category).ToList();
                return Page();
            }
            else
            {
                return RedirectToPage("/SecurePage");

            }         
        }
    }
}
