using Microsoft.Extensions.FileProviders;
using RestaurantManagement.Models;
using RestaurantManagement.ResHub;

namespace RestaurantManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddScoped<RestaurantManagementContext>();
            builder.Services.AddDbContext<RestaurantManagementContext>();
            builder.Services.AddSignalR();
			builder.Services.AddHttpContextAccessor();


			builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(1800);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {

                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
    Path.Combine(Directory.GetCurrentDirectory(), "Image")),
                RequestPath = "/Image"
            });
            // session
            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();
            
            app.MapRazorPages();
            app.MapHub<ResHub.ResHub>("/resHub");
            app.Run();
        }
    }
}