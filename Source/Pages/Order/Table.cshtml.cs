using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;
using RestaurantManagement.ResHub;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace RestaurantManagement.Pages.Order
{
    public class TableModel : PageModel
    {
        private readonly RestaurantManagementContext _context;
        private readonly IHubContext<ResHub.ResHub> _hubContext;

        public TableModel(RestaurantManagementContext context, IHubContext<ResHub.ResHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public List<Models.TableOrder> tableOrders =new List<Models.TableOrder>();
        public List<Models.TableOrderCustomer> tableOrderCustomers =new List<Models.TableOrderCustomer>();
        public IActionResult OnGet()
        {

            if (HttpContext.Session.GetString("IsAuthenticated") != "true")
            {
                // User is authenticated, you can redirect to a secured page
                return RedirectToPage("/SecurePage");
            }
            else
            {
                tableOrders = _context.TableOrders.ToList();
                var now = DateTime.Now.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                var endOfDay = now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                 tableOrderCustomers = _context.TableOrderCustomers
                    .Include("TableOrder")
                    .Where(t => t.Start > now && t.End < endOfDay)
                    .ToList();
                return Page();
            }

          

            
            
        }
        public String message;
        public async Task<IActionResult> OnPost(int table_id, DateTime start, DateTime end)
        {
           
            if (start > end|| start < DateTime.Now )
            {
                tableOrders = _context.TableOrders.ToList();
                var now = DateTime.Now.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                var endOfDay = now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                tableOrderCustomers = _context.TableOrderCustomers
                    .Include("TableOrder")
                    .Where(t => t.Start > now && t.End < endOfDay)
                    .ToList();
                message = "Time is not valid";
                return Page();
            }



            // get customer_id from session
            if(!IsTableBooked(table_id, start, end))
            {
                _context.TableOrderCustomers.Add(new TableOrderCustomer
                {
                    TableOrderId = table_id,
                    Start = start,
                    End = end,
                    CustomerId = int.Parse(HttpContext.Session.GetString("id"))
                }); ;
                _context.SaveChanges();
                TableOrderCustomer tableOrderCustomer = _context.TableOrderCustomers.Include("TableOrder")
                    .FirstOrDefault(x => x.CustomerId == int.Parse(HttpContext.Session.GetString("id"))
                    && x.TableOrderId == table_id && x.Start == start);
                HttpContext.Session.SetString("TableOrderCustomerId", tableOrderCustomer.Id != null ? tableOrderCustomer.Id + "" : 0 + "");
               
                await _hubContext.Clients.All.SendAsync("LoadTableBooked", "loading");
                return RedirectToPage("/order/food");
            }
            else
            {
                tableOrders = _context.TableOrders.ToList();
                var now = DateTime.Now.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
                var endOfDay = now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

                tableOrderCustomers = _context.TableOrderCustomers
                   .Include("TableOrder")
                   .Where(t => t.Start > now && t.End < endOfDay)
                   .ToList();
                message = "Table in time has been booked by someone";
                return Page();
            }

             
            
        }



        public bool IsTableBooked(int tableId, DateTime startTime, DateTime endTime)
        {
            foreach (var reservation in _context.TableOrderCustomers.ToList())
            {
                if (reservation.TableOrderId == tableId)
                {
                    if (startTime < reservation.End && endTime > reservation.Start)
                    {
                        // There is an overlap; check for different scenarios
                        if (startTime <= reservation.Start  && endTime >= reservation.End )
                        {
                            // Requested time range completely covers an existing reservation
                            return true;
                        }
                        else if (startTime >= reservation.Start && endTime <= reservation.End )
                        {
                            // Requested time range is completely within an existing reservation
                            return true;
                        }
                        else if (startTime <= reservation.Start  && endTime <= reservation.End )
                        {
                            // Requested time range starts before and ends within an existing reservation
                            return true;
                        }
                        else if (startTime >= reservation.Start  && endTime >= reservation.End )
                        {
                            // Requested time range starts within and ends after an existing reservation
                            return true;
                        }
                    }
                }
            }

            // No overlap found; the table is available for the requested time range
            return false;
        }

    }
}
