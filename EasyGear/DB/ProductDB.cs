using DAL.DAO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL.DB
{
    public class ProductDB : ProductDAO
    {
        public string ConnectionString { get; private set; }
        private SqlConnection _connection;

        public ProductDB(string connectionString)
        {
            ConnectionString = connectionString;
            _connection = new SqlConnection(connectionString);
        }

        public void AddProduct(Product product)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("INSERT INTO Product (ProductName, Price) VALUES (@ProductName, @Price); SELECT SCOPE_IDENTITY();", connection))
                {
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@Price", product.Price);

                    
                    product.ProductID = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
        }

        public bool DeleteProduct(int productID)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("DELETE FROM Product WHERE ProductID = @ProductID;", connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productID);
                    bool success = command.ExecuteNonQuery() > 0;
                    connection.Close();
                    return success;
                }
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            var products = new List<Product>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT ProductID, ProductName, Price FROM Product;", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductID = reader.GetInt32(0),
                                ProductName = reader.GetString(1),
                                Price = reader.GetInt32(2)
                            });
                        }
                    }
                }
            }

            return products;
        }

        public Product GetProductById(int productID)
        {
            Product product = null;

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                
                using (var command = new SqlCommand("SELECT ProductID, ProductName, Price FROM Product WHERE ProductID = @ProductID;", connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productID);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            
                            int id = reader.GetInt32(0);
                            string productName = reader.GetString(1);
                            int price = reader.GetInt32(2);

                            
                            product = new Product(id, productName, price);
                        }
                    }
                }
            }

            return product;
        }


        public bool UpdateProduct(Product product)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("UPDATE Product SET ProductName = @ProductName WHERE ProductID = @ProductID;", connection))
                {
                    command.Parameters.AddWithValue("@ProductID", product.ProductID);
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    bool success = command.ExecuteNonQuery() > 0;
                    connection.Close();
                    return success;
                }
            }
        }
    }
}
