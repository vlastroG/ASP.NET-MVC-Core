using AddIns;

namespace RazorPagesGB.Models
{
    public class Catalog
    {
        private ConcurrentList<Product> _products { get; set; } = new();
        private readonly object _syncProducts = new();
        public void ProductAdd(Product product)
        {
            lock (_syncProducts)
            {
                _products.Add(product);
            }
        }

        public void ProductDelete(int id)
        {
            lock (_syncProducts)
            {
                if (_products.FirstOrDefault(p => p.Id == id) != null)
                {
                    var product = _products.FirstOrDefault(p => p.Id == id);
                    _products.Remove(product);
                }
            }
        }

        public void Clear()
        {
            lock (_syncProducts)
            {
                _products.Clear();
            }
        }

        public IReadOnlyList<Product> ProductsGetAll()
        {
            var products_readonly = _products.GetAll() as IReadOnlyList<Product>;
            return products_readonly;
        }
    }
}
