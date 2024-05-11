using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;

namespace RestaurantManagement.Pages.Order
{
    public class ListOrderDetailModel : PageModel
    {

        private readonly RestaurantManagementContext _context;
        public ListOrderDetailModel(RestaurantManagementContext context)
        {
            _context = context;
        }
        public List<TableOrderCustomer> tableOrderCustomers = new List<TableOrderCustomer> { };

        public List<Models.TableOrder> tableOrders = new List<Models.TableOrder> { };
        public List<FoodTable> foodTables = new List<FoodTable> { };
        public List<Food> foods = new List<Food> { };
        public List<Models.Combo> combos = new List<Models.Combo> { };
        public Models.Customer customer = new Models.Customer();
        public IActionResult OnGet()
        {

            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                // User is authenticated, you can redirect to a secured page
                return RedirectToPage("/SecurePage");
            }
            else
            {

                customer = _context.Customers.Include("TableOrderCustomers")
                .FirstOrDefault(x => x.Username == HttpContext.Session.GetString("Username"));
                if (customer != null)
                {

                    tableOrderCustomers = customer.TableOrderCustomers.ToList();
                    foods = _context.Foods.ToList();
                    combos = _context.Combos.ToList();
                    tableOrderCustomers.ForEach(x =>
                    {
                        foodTables = _context.FoodTables.Where(y => y.TableOrderCustomerId == x.Id).ToList();

                        Models.TableOrder tableOrder = _context.TableOrders.FirstOrDefault(y => y.Id == x.TableOrderId);
                        tableOrders.Add(tableOrder);
                    });
                }
                return Page();
            }





        }

        public IActionResult OnPost(int id)
        {
            TableOrderCustomer x = _context.TableOrderCustomers.FirstOrDefault(ii => ii.Id == id);
            _context.TableOrderCustomers.Remove(x);
            List<FoodTable> foodTables = _context.FoodTables.Where(q => q.TableOrderCustomerId == id).ToList();
            _context.FoodTables.RemoveRange(foodTables);
            _context.SaveChanges();
            Console.WriteLine(id);



            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                // User is authenticated, you can redirect to a secured page
                return RedirectToPage("/SecurePage");
            }
            else
            {

                customer = _context.Customers.Include("TableOrderCustomers")
                .FirstOrDefault(x => x.Username == HttpContext.Session.GetString("Username"));
                if (customer != null)
                {

                    tableOrderCustomers = customer.TableOrderCustomers.ToList();
                    foods = _context.Foods.ToList();
                    combos = _context.Combos.ToList();
                    tableOrderCustomers.ForEach(x =>
                    {
                        foodTables = _context.FoodTables.Where(y => y.TableOrderCustomerId == x.Id).ToList();

                        Models.TableOrder tableOrder = _context.TableOrders.FirstOrDefault(y => y.Id == x.TableOrderId);
                        tableOrders.Add(tableOrder);
                    });
                }

                return Page();
            }
        }
    }
      
}
