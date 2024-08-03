using DAL.Models;
using System.Collections.Generic;

namespace DAL.DAO
{
    public interface ProductDAO
    {
        void AddProduct(Product product);
        bool DeleteProduct(int id);
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);
        bool UpdateProduct(Product product);
    }
}

