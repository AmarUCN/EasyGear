using DAL.DAO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DB
{
    public class ProductLineDB : ProductLineDAO
    {
        public string ConnectionString { get; private set; }
        private SqlConnection _connection;

        public ProductLineDB(string connectionString)
        {
            ConnectionString = connectionString;
            _connection = new SqlConnection(connectionString);
        }


        public IEnumerable<ProductLine> GetAllProductLines()
        {
            var productLines = new List<ProductLine>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT pl.ProductLineID, pl.ProductID, pl.OrderNumber, pl.Quantity, p.ProductName, p.Price " +
                                                   "FROM ProductLine pl " +
                                                   "JOIN Product p ON pl.ProductID = p.ProductID;", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productLines.Add(new ProductLine
                            {
                                ProductLineID = reader.GetInt32(0),
                                ProductID = reader.GetInt32(1),
                                OrderNumber = reader.GetInt32(2),
                                Quantity = reader.GetInt32(3),
                                
                            });
                        }
                    }
                }
            }

            return productLines;
        }

        public ProductLine? GetProductLineById(int productLineID)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT pl.ProductLineID, pl.ProductID, pl.OrderNumber, pl.Quantity, p.ProductName, p.Price " +
                                                   "FROM ProductLine pl " +
                                                   "JOIN Product p ON pl.ProductID = p.ProductID " +
                                                   "WHERE pl.ProductLineID = @ProductLineID;", connection))
                {
                    command.Parameters.AddWithValue("@ProductLineID", productLineID);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ProductLine
                            {
                                ProductLineID = reader.GetInt32(0),
                                ProductID = reader.GetInt32(1),
                                OrderNumber = reader.GetInt32(2),
                                Quantity = reader.GetInt32(3)
                                // Price and ProductName handled in the model or elsewhere
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void AddProductLine(ProductLine productLine)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO ProductLine (ProductID, OrderNumber, Quantity) VALUES (@ProductID, @OrderNumber, @Quantity); SELECT SCOPE_IDENTITY();", connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productLine.ProductID);
                    command.Parameters.AddWithValue("@OrderNumber", productLine.OrderNumber);
                    command.Parameters.AddWithValue("@Quantity", productLine.Quantity);
                    productLine.ProductLineID = Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public bool DeleteProductLine(int productLineID)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM ProductLine WHERE ProductLineID = @ProductLineID;", connection))
                {
                    command.Parameters.AddWithValue("@ProductLineID", productLineID);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool UpdateProductLine(ProductLine productLine)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE ProductLine SET Quantity = @Quantity WHERE ProductID = @ProductID;", connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productLine.ProductID);
                    command.Parameters.AddWithValue("@Quantity", productLine.Quantity);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}

