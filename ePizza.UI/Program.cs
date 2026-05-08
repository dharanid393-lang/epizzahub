using ePizza.UI.Models;
using ePizza.UI.RazorPay;
using ePizza.UI.TokenHelpers;
using ePizza.UI.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace ePizza.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //This code will automatically redirect to login page if user is not authenticated
            //and we need to use authorize word in the controller
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                        .AddCookie(options =>
                        {
                            options.LoginPath = "/Login/Login";
                            options.LogoutPath = "/Login/Logout";
                        });

            builder.Services.AddSession(options =>
            {
                options.IOTimeout = TimeSpan.FromMinutes(20);
            });

            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthorization();

            builder.Services.AddScoped<TokenHandler>();
            builder.Services.AddTransient<IRazorPayService, RazorPayService>();


            // this will read the configuration from appsettings.json file and map to the model
            builder.Services.Configure<RazorPayConfigurationModel>(
                builder.Configuration.GetSection("RazorPay"));

            //builder.Services.AddHttpClient(ApplicationConstants.EPizzaClientName, option =>
            //{
            //    option.BaseAddress = new Uri(builder.Configuration["ePizzaApi:Url"]!);
            //    option.DefaultRequestHeaders.Add("Accept", "application/json");
            //})
            //.AddHttpMessageHandler<TokenHelpers.TokenHandler>();

            builder.Services.AddHttpClient(ApplicationConstants.EPizzaClientName, option =>
            {
                option.BaseAddress = new Uri(builder.Configuration["ePizzaApi:Url"]!);
                option.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddHttpMessageHandler<TokenHandler>();

            builder.Services.AddTransient<ITokenService, TokenService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();
            app.UseAuthentication();
           

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
