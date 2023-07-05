using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Runtime.CompilerServices;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryAvailability = retry.Value;

            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating Postgresql database.");

                    using var connection = new NpgsqlConnection
                        (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

                    connection.Open();

                    using var command = new NpgsqlCommand
                    {
                        Connection = connection

                    };

                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY,
                                                                ProductName VARCHAR(100) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
                    command.ExecuteNonQuery();

                    command.CommandText = @"INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('Purnbramha', 'Purnbramha Discount', 100)";
                    command.ExecuteNonQuery();

                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An error occured while migrating the postgresql database.");

                    if(retryAvailability < 50)
                    {
                        retryAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryAvailability);
                    }
                }

                return host;

            }
        }
    }
}
