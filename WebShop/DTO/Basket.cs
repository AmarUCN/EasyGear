using DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace WebShop.DTO
{
    public class Basket
    {
        public Dictionary<int, ProductLine> ProductLines { get; set; }

        public Basket(Dictionary<int, ProductLine>? productLines = null)
        {
            ProductLines = productLines ?? new Dictionary<int, ProductLine>();
        }

        // Adds or updates the quantity of a product in the basket
        public void AddOrUpdateProduct(ProductLine productLine)
        {
            if (ProductLines.ContainsKey(productLine.ProductID))
            {
                ProductLines[productLine.ProductID].Quantity += productLine.Quantity;
                if (ProductLines[productLine.ProductID].Quantity <= 0)
                {
                    ProductLines.Remove(productLine.ProductID);
                }
            }
            else
            {
                ProductLines[productLine.ProductID] = productLine;
            }
        }

        // Removes a product from the basket
        public void RemoveProduct(int productID)
        {
            ProductLines.Remove(productID);
        }

        // Updates the quantity of a specific product
        public void UpdateProductQuantity(int productID, int quantity)
        {
            if (ProductLines.ContainsKey(productID))
            {
                ProductLines[productID].Quantity = quantity;
                if (ProductLines[productID].Quantity <= 0)
                {
                    ProductLines.Remove(productID);
                }
            }
        }

        #region Helper Methods

        // Returns the total number of products in the basket
        public int GetTotalProductCount() => ProductLines.Sum(pl => pl.Value.Quantity);

        // Empties the basket
        public void Clear() => ProductLines.Clear();

        #endregion
    }
}
