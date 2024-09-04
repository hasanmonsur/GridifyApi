using Gridify;
using WebApiGridify.Models;

namespace WebApiGridify.Contacts
{
    public interface IProductRepository
    {
        void AddProduct(Product product);
        void DeleteProduct(int id);
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);

        List<Product> GetProducts(GridifyQuery gridifyQuery);
        List<Product> GetFilteredProducts(GridifyQuery gridifyQuery);
        void UpdateProduct(Product product);
    }
}