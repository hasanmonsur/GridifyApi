using Dapper;
using Gridify;
using WebApiGridify.Contacts;
using WebApiGridify.Helpers;
using WebApiGridify.Models;
using System.Linq;

namespace WebApiGridify.Repositorys
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperContext _context;


        public ProductRepository(DapperContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            using (var connection = _context.CreateConnection())
            {
                // Dapper will automatically map the result to the Product model
                var products = connection.Query<Product>("SELECT * FROM Products").ToList();
                return products;
            }
        }

        public Product GetProductById(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var product = connection.QuerySingleOrDefault<Product>("SELECT * FROM Products WHERE Id = @Id", new { Id = id });
                return product;
            }
        }

        public List<Product> GetProducts(GridifyQuery gridifyQuery)
        {
            using (var connection = _context.CreateConnection())
            {
                var products = connection.Query<Product>("SELECT * FROM Products").AsQueryable().Gridify(gridifyQuery);
                return products.Data.ToList();
            }
        }

        public List<Product> GetFilteredProducts(GridifyQuery gridifyQuery)
        {
            using (var connection = _context.CreateConnection())
            {                
                var products = connection.Query<Product>("SELECT * FROM Products").AsQueryable().ApplyFiltering(gridifyQuery);

                return products.ToList();
            }
        }

        public void AddProduct(Product product)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = "INSERT INTO Products (Name, Price) VALUES (@Name, @Price)";
                connection.Execute(sql, product);
            }
        }

        public void UpdateProduct(Product product)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = "UPDATE Products SET Name = @Name, Price = @Price WHERE Id = @Id";
                connection.Execute(sql, product);
            }
        }

        public void DeleteProduct(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = "DELETE FROM Products WHERE Id = @Id";
                connection.Execute(sql, new { Id = id });
            }
        }
    }
}
