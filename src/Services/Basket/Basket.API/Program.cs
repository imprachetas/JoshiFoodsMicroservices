using Basket.API.gRPCServices;
using Basket.API.Repositories;
using Discount.Grpc.Protos;
using MassTransit;

namespace Basket.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddAutoMapper(typeof(Program));
            
            builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
                (o => o.Address = new Uri(builder.Configuration["gRPCSettings:DiscountURL"]));

            builder.Services.AddScoped<DiscountgRPCService>();

            builder.Services.AddMassTransit(config => {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}