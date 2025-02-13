using DK.Ordering.Core.Entities;

namespace DK.Ordering.Core.Contracts
{
    public interface IOrderRepository : IAsyncRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
    }
}
