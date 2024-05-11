using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RestaurantManagement.Pages.Authentication
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            
                // Log out the user (clear authentication status)
                HttpContext.Session.Remove("IsAuthenticated");

                return RedirectToPage("/Authentication/Login");

        }
    }
}
