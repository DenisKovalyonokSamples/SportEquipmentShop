﻿using AutoMapper;
using DK.Ordering.Application.Commands;
using DK.Ordering.Core.Contracts;
using DK.Ordering.Core.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DK.Ordering.Application.Handlers
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            var generatedOrder = await _orderRepository.AddAsync(orderEntity);

            _logger.LogInformation(($"Order {generatedOrder} successfully created."));
            
            return generatedOrder.Id;
        }
    }
}
