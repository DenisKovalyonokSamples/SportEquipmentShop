using DK.Ordering.Application.Commands;
using DK.Ordering.Application.Exceptions;
using DK.Ordering.Core.Contracts;
using DK.Ordering.Core.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DK.Ordering.Application.Handlers
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger<DeleteOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }
        public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToDelete = await _orderRepository.GetByIdAsync(request.Id);
            if (orderToDelete == null)
            {
                throw new OrderNotFoundException(nameof(Order), request.Id);
            }

            await _orderRepository.DeleteAsync(orderToDelete);
            
            _logger.LogInformation($"Order with Id {request.Id} is deleted successfully.");
        }
    }
}
