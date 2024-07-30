using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public interface ProductLineDAO
    {
        void AddProductLine(ProductLine productLine);
        bool DeleteProductLine(int productLineID);
        IEnumerable<ProductLine> GetAllProductLines();
        ProductLine? GetProductLineById(int productLineID);
        bool UpdateProductLine(ProductLine productLine);
    }
}
