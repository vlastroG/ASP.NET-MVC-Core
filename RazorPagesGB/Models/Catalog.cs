using AddIns;

namespace RazorPagesGB.Models
{
    public class Catalog
    {
        private ConcurrentList<Product> _products { get; set; } = new();
        public void ProductAdd(Product product)
        {
            _products.Add(product);
        }

        public void ProductDelete(int id)
        {
            if (_products.FirstOrDefault(p => p.Id == id) != null)
            {
                var product = _products.FirstOrDefault(p => p.Id == id);
                _products.Remove(product);
            }
        }

        public void Clear()
        {
            _products.Clear();
        }

        public IReadOnlyList<Product> ProductsGetAll()
        {
            var productsReadonly = _products.GetAll() as IReadOnlyList<Product>;
            return productsReadonly;
        }
    }
}
