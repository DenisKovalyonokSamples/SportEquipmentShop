using DK.Ordering.Core.Common;
using DK.Ordering.Core.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace DK.Ordering.Infrastructure.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = "system"; //TODO: Should be replaced by Identity Server
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModifiedBy = "system"; //TODO: Should be replaced by Identity Server
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    var decimalProps = modelBuilder.Model
        //    .GetEntityTypes()
        //    .SelectMany(t => t.GetProperties())
        //    .Where(p => (System.Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));

        //    foreach (var property in decimalProps)
        //    {
        //        property.SetPrecision(18);
        //        property.SetScale(2);
        //    }
        //}
    }
}
