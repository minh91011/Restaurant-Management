using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace RestaurantManagement.ResHub
{
    public class ResHub: Hub
    {

        private readonly RestaurantManagementContext _context;
        public ResHub(RestaurantManagementContext context)
        {
            _context = context;
        }


        public async Task DeleteTableOrder(int id)
        {
            TableOrder tableOrder = _context.TableOrders.FirstOrDefault(f=>f.Id==id);
            _context.TableOrders.Remove(tableOrder);
            _context.SaveChanges();
            await Clients.All.SendAsync("LoadTableOrder",_context.TableOrders.ToList());
        }
        public async Task LoadBook()
        {
           // var now = DateTime.Now.Date.AddHours(0).AddMinutes(0).AddSeconds(0);
           // var endOfDay = now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

           //List<TableOrderCustomer>   tableOrderCustomers = _context.TableOrderCustomers
           //     .Include("TableOrder")
           //     .Where(t => t.Start > now && t.End < endOfDay)
           //     .ToList();
            await Clients.All.SendAsync("LoadTableBooked", "loading");
        }

    }
    
}
