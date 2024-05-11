using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RestaurantManagement.Pages.Authentication
{
    public class OTPModel : PageModel
    {   public String message;
        public void OnGet()
        {
        }
       
        public IActionResult OnPost(String Otp)
        {
            if (HttpContext.Session.GetString("OTP_REGISTER") == Otp)
            {
                HttpContext.Session.SetString("EmailConfirmed", "true");
                return RedirectToPage("/Authentication/Register");
            }
            else if (HttpContext.Session.GetString("OTP_RESETPASSWORD") == Otp) {
				return RedirectToPage("/Authentication/ResetPassword");
			}
            else
            {
                message = "OTP is wrong! Please Check again!";
                return Page();
            }
              
        }
    }
}
