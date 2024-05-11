using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestaurantManagement.Models;

namespace RestaurantManagement.Pages.Authentication
{
    public class ResetPasswordModel : PageModel
    {
		private readonly RestaurantManagementContext _context;
		public ResetPasswordModel(RestaurantManagementContext context)
		{
			_context = context;
		}
		public string message;
		public void OnGet()
        {
        }

        public IActionResult OnPost(string NewPassword, string ConfirmPassword)
        {
			if(NewPassword != ConfirmPassword)
			{
				message = "confirm password không trùng với new password";
				return Page();
			}
			else
			{
				Models.Customer c = _context.Customers.Where(cu => cu.Email == HttpContext.Session.GetString("EMAIL_RESETPASSWORD")).FirstOrDefault();
				if (c != null)
				{
					c.Password = NewPassword;
					_context.Customers.Update(c);
					_context.SaveChanges();
                    return RedirectToPage("/Authentication/Login");
                }
			}
            message = "co loi";
            return Page();

        }
    }
}
