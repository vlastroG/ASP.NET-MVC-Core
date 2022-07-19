using RazorPagesGB.Domain.DomainEvents;
using RazorPagesGB.Models;

namespace RazorPagesGB.Domain
{
    public class ProductAdded : IDomainEvent
    {
        public Product Product { get; }


        public ProductAdded(Product product)
        {
            Product = product;
        }
    }
}
