using AutoMapper;
using DK.EventBus.Messages.Events;
using DK.Ordering.Application.Commands;
using MassTransit;
using MediatR;

namespace DK.Ordering.API.EventBusConsumer
{
    public class BasketOrderingConsumerV2 : IConsumer<BasketCheckoutEventV2>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketOrderingConsumerV2> _logger;

        public BasketOrderingConsumerV2(IMediator mediator, IMapper mapper, ILogger<BasketOrderingConsumerV2> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEventV2> context)
        {
            using var scope = _logger.BeginScope("Consuming Basket Checkout Event for {correlationId}",
                context.Message.CorrelationId);

            var command = _mapper.Map<CheckoutOrderCommand>(context.Message);
            PopulateAddressDetails(command);
            var result = await _mediator.Send(command);
            
            _logger.LogInformation($"Basket checkout event completed!!!");
        }

        private static void PopulateAddressDetails(CheckoutOrderCommand command)
        {
            command.FirstName = "System";
            command.LastName = "Shop";
            command.EmailAddress = "system@eshop.net";
            command.AddressLine = "Barcelona";
            command.Country = "Spain";
            command.State = "CT";
            command.ZipCode = "08002";
            command.PaymentMethod = 1;
            command.CardName = "Visa";
            command.CardNumber = "0123456789012345";
            command.Expiration = "12/27";
            command.CVV = "123";
        }
    }
}
