using System.ComponentModel.DataAnnotations.Schema;

namespace DK.EventBus.Messages.Events
{
    public class BasketCheckoutEventV2 : BaseIntegrationEvent
    {
        public string? UserName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalPrice { get; set; }
    }
}
