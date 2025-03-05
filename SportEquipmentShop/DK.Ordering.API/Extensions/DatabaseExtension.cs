using DK.Ordering.Infrastructure.Data;
using DK.Ordering.Infrastructure.Data.Seed;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace DK.Ordering.API.Extensions
{
    public static class DatabaseExtension
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();

                try
                {
                    logger.LogInformation($"Started Db Migration: {typeof(TContext).Name}");

                    var retry = Policy.Handle<SqlException>()
                        .WaitAndRetry(
                            retryCount: 2,
                            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                            onRetry: (exception, span, cont) =>
                            {
                                logger.LogError("Retrying because of {exception} {retry}", exception, span);
                            });
                    retry.Execute(() => CallSeeder(seeder, context, services));

                    logger.LogInformation($"Migration Completed: {typeof(TContext).Name}");
                }
                catch (SqlException e)
                {
                    logger.LogError(e, $"An error occurred while migrating db: {typeof(TContext).Name}");
                }
            }

            return host;
        }

        private static void CallSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
