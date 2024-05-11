using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestaurantManagement.Models;

namespace RestaurantManagement.Pages.TableOrder
{
    public class ListTableOrderModel : PageModel
    {
        private readonly RestaurantManagementContext _context;
        public ListTableOrderModel(RestaurantManagementContext context)
        {
            _context = context;
        }
        public List<Models.TableOrder> tableOrders{ get; set; }

        public void OnGet()
        {
            tableOrders = _context.TableOrders.ToList();
        }
    }
}
