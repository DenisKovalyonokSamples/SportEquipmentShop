using DK.Ordering.Core.Entities;
using Microsoft.Extensions.Logging;

namespace DK.Ordering.Infrastructure.Data.Seed
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetOrders());
                await orderContext.SaveChangesAsync();

                logger.LogInformation($"Ordering Database: {typeof(OrderContext).Name} seeded.");
            }
        }

        private static IEnumerable<Order> GetOrders()
        {
            return new List<Order>
        {
            new()
            {
                UserName = "system",
                FirstName = "System",
                LastName = "Shop",
                EmailAddress = "system@eshop.net",
                AddressLine = "Barcelona",
                Country = "Spain",
                TotalPrice = 750,
                State = "CT",
                ZipCode = "08002",
                CardName = "Visa",
                CardNumber = "0123456789012345",
                CreatedBy = "System",
                Expiration = "12/27",
                Cvv = "123",
                PaymentMethod = 1,
                LastModifiedBy = "System",
                LastModifiedDate = DateTime.UtcNow,
            },
            new()
            {
                UserName = "denis",
                FirstName = "Denis",
                LastName = "Kov",
                EmailAddress = "deniskov@eshop.net",
                AddressLine = "Malaga",
                Country = "Spain",
                TotalPrice = 850,
                State = "AN",
                ZipCode = "29003",

                CardName = "Visa",
                CardNumber = "1234567890123456",
                CreatedBy = "Denis",
                Expiration = "12/29",
                Cvv = "125",
                PaymentMethod = 1,
                LastModifiedBy = "Denis",
                LastModifiedDate = DateTime.UtcNow,
            }
        };
        }
    }
}
