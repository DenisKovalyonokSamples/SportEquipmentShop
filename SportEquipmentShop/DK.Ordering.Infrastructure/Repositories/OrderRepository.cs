using DK.Ordering.Core.Contracts;
using DK.Ordering.Core.Entities;
using DK.Ordering.Infrastructure.Data;
using DK.Ordering.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DK.Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            var orderList = await _dbContext.Orders
                .Where(o => o.UserName == userName)
                .ToListAsync();

            return orderList;
        }
    }
}
