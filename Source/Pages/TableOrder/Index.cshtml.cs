using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestaurantManagement.Models;

namespace RestaurantManagement.Pages.TableOrder
{
    public class IndexModel : PageModel
    {
        private readonly RestaurantManagementContext _context;
        public IndexModel(RestaurantManagementContext context)
        {
            _context=context;
        }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("IsAdmin") == "true")
            {
                 
                return Page();
            }
            else
            {
                return RedirectToPage("/SecurePage");

            }
        }
        public void OnPost(String description , int floor, int number)
        {
            _context.Add(new Models.TableOrder
            {
                Description = description,
                Floor = floor,
                Number = number
            });
            _context.SaveChanges();
            Console.WriteLine("test");
        }
    }
}
