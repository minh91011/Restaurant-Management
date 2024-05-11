using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestaurantManagement.Models;

namespace RestaurantManagement.Pages.Authentication
{
    public class RegisterModel : PageModel
    {
        private readonly RestaurantManagementContext _context;
        public RegisterModel(RestaurantManagementContext context)
        {
            _context = context;
        }
        public String email;
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("EmailConfirmed") == "true")
            {
                email = HttpContext.Session.GetString("EMAIL_REGISTER" );
                return Page();
            }
            else
            {
                return RedirectToPage("/Authentication/MailConfirm");
            }
        }
        public IActionResult OnPost(Models.Customer c)
        {
            _context.Customers.Add(c);
            _context.SaveChanges();

            return RedirectToPage("/Authentication/Login");

        }
    }
}
