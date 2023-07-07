using AspnetRunBasics.Services;

namespace AspnetRunBasics6
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }

        private static void RegisterHttpClients(WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
                    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]));

            builder.Services.AddHttpClient<IBasketService, BasketService>(c =>
                    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BasketUrl"]));

            builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
                    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderingUrl"]));


        }
    }
}