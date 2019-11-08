using System;
using U.EventBus.Events;

namespace U.ProductService.Application.Events.IntegrationEvents.Events
{
    public class ProductPropertiesChangedIntegrationEvent : IntegrationEvent
    {
        public Guid ProductId { get; }
        
        public string Name { get; }
        
        public decimal Price { get; }
        

        public ProductPropertiesChangedIntegrationEvent(Guid productId, string name, decimal price)
        {
            ProductId = productId;
            Name = name;
            Price = price;
        }
    }
}