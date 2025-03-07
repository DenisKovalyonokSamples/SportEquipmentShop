﻿using AutoMapper;
using DK.EventBus.Messages.Events;
using DK.Ordering.Application.Commands;
using MassTransit;
using MediatR;

namespace DK.Ordering.API.EventBusConsumer
{
    public class BasketOrderingConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketOrderingConsumer> _logger;

        public BasketOrderingConsumer(IMediator mediator, IMapper mapper, ILogger<BasketOrderingConsumer> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            using var scope = _logger.BeginScope("Consuming Basket Checkout Event for {correlationId}",
                context.Message.CorrelationId);
            
            var command = _mapper.Map<CheckoutOrderCommand>(context.Message);
            var result = await _mediator.Send(command);
            
            _logger.LogInformation($"Basket checkout event completed!!!");
        }
    }
}
