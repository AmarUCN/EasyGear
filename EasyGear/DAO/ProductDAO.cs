using DAL.Models;
using System.Collections.Generic;

namespace DAL.DAO
{
    public interface ProductDAO
    {
        void AddProduct(Product product);
        bool DeleteProduct(int productID);
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int productID);
        bool UpdateProduct(Product product);
    }
}
