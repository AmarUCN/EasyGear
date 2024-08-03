using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.DAO;
using DAL.Models;

namespace DAL.DB
{
    public class ProductDB : ProductDAO
    {
        public string ConnectionString { get; private set; }

        public ProductDB(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void AddProduct(Product product)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var query = "INSERT INTO Product (ProductName, Price) VALUES (@ProductName, @Price); SELECT SCOPE_IDENTITY();";
                    using (var command = new SqlCommand(query, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@ProductName", product.ProductName);
                        command.Parameters.AddWithValue("@Price", product.Price);
                        product.Id = Convert.ToInt32(command.ExecuteScalar());
                        transaction.Commit();
                    }
                }
            }
        }

        public bool DeleteProduct(int productID)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var query = "DELETE FROM Product WHERE ProductID = @ProductID;";
                    using (var command = new SqlCommand(query, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@ProductID", productID);
                        var result = command.ExecuteNonQuery();
                        transaction.Commit();
                        return result > 0;
                    }
                }
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            var products = new List<Product>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var query = "SELECT ProductID, ProductName, Price FROM Product WITH (UPDLOCK, ROWLOCK);";
                    using (var command = new SqlCommand(query, connection, transaction))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products.Add(new Product
                                {
                                    Id = reader.GetInt32(0),
                                    ProductName = reader.GetString(1),
                                    Price = reader.GetInt32(2)
                                });
                            }
                        }
                    }
                    transaction.Commit();
                }
            }

            return products;
        }

        public Product? GetProductById(int productID)
        {
            Product? product = null;

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    // Simplified query without 'WITH' clause
                    var query = "SELECT ProductID, ProductName, Price FROM Product WHERE ProductID = @ProductID;";
                    using (var command = new SqlCommand(query, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@ProductID", productID);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                product = new Product
                                {
                                    Id = reader.GetInt32(0),
                                    ProductName = reader.GetString(1),
                                    Price = reader.GetInt32(2)  
                                };
                            }
                        }
                    }
                    transaction.Commit();
                }
            }

            return product;
        }


        public bool UpdateProduct(Product product)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var query = "UPDATE Product SET ProductName = @ProductName, Price = @Price WHERE ProductID = @ProductID;";
                    using (var command = new SqlCommand(query, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@ProductID", product.Id);
                        command.Parameters.AddWithValue("@ProductName", product.ProductName);
                        command.Parameters.AddWithValue("@Price", product.Price);
                        var result = command.ExecuteNonQuery();
                        transaction.Commit();
                        return result > 0;
                    }
                }
            }
        }
    }
}



