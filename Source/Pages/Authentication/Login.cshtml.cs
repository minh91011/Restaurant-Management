using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RestaurantManagement.Models;

namespace RestaurantManagement.Pages.Authentication
{
    public class LoginModel : PageModel
    {

        private readonly RestaurantManagementContext _context;
        public LoginModel(RestaurantManagementContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("IsAuthenticated" )!= null)
            {
                return RedirectToPage("/Order/Table");
            }
            else
            {
                return Page();
            }
        }
        public String message;
        public IActionResult OnPost(String Email, String Password)
        {
            Models.Customer customer = _context.Customers.FirstOrDefault(x => x.Email == Email && x.Password == Password);
           if (_context.Customers.FirstOrDefault(x => x.Email == Email && x.Password == Password)!= null)
            {
                if(Email == "admin@gmail.com")
                {
                    HttpContext.Session.SetString("IsAdmin", "true");
                    return RedirectToPage("/foodmanager/index");
                }
                HttpContext.Session.SetString("IsAuthenticated", "true");
                HttpContext.Session.SetString("Username", customer.Username);
                HttpContext.Session.SetString("Email", Email);
                HttpContext.Session.SetString("id",customer.Id +"");
                return RedirectToPage("/Order/Table");
            }
            else
            {
                message = "Login Failed";
                return Page();
            }

        }
    }
}
