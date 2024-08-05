using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Numerics;
using System.Threading.Tasks;
using DAL.DAO;
using DAL.Models;

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

        public void AddProductLine(ProductLine productLine)
        {
            SqlTransaction transaction = null;
            try
            {
                _connection.Open();
                transaction = _connection.BeginTransaction();

                using (var command = new SqlCommand("INSERT INTO ProductLine (Quantity, ProductID, BasketID, OrderNumber) VALUES (@Quantity, @ProductID, @BasketID, @OrderNumber); SELECT SCOPE_IDENTITY();", _connection, transaction))
                {
                    command.Parameters.AddWithValue("@Quantity", productLine.Quantity);
                    command.Parameters.AddWithValue("@ProductID", productLine.ProductID);
                    command.Parameters.AddWithValue("@BasketID", (object)productLine.BasketID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@OrderNumber", (object)productLine.OrderNumber ?? DBNull.Value);

                    productLine.Id = Convert.ToInt32(command.ExecuteScalar());
                }

                // Commit transaction
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Rollback transaction if there is an error
                if (transaction != null)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception rollbackEx)
                    {
                        Console.WriteLine($"Error rolling back transaction: {rollbackEx.Message}");
                    }
                }

                Console.WriteLine($"Error inserting ProductLine: {ex.Message}");
                // Consider logging the exception to a file or monitoring system
            }
            finally
            {
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }



        public bool DeleteProductLine(int id)
        {
            try
            {
                _connection.Open();

                string query = "DELETE FROM ProductLine WHERE ProductLineID = @id";

                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.Close();
            }

            return false;
        }

        public IEnumerable<ProductLine> GetAllProductLines()
        {
            List<ProductLine> productLines = new List<ProductLine>();
            string query = "SELECT ProductLineID, Quantity, ProductID, BasketID, OrderNumber FROM ProductLine";

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int productLineId = reader.GetInt32(0);
                    int quantity = reader.GetInt32(1);
                    int productId = reader.GetInt32(2);
                    int? basketId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3);
                    int? orderNumber = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4);

                    ProductLine productLine = new ProductLine(productLineId, productId, quantity, basketId, orderNumber);
                    productLines.Add(productLine);
                }

                _connection.Close();
            }

            return productLines;
        }

        public ProductLine? GetProductLineById(int id)
        {
            try
            {
                _connection.Open();

                string query = "SELECT ProductLineID, Quantity, ProductID, BasketID, OrderNumber FROM ProductLine WHERE ProductLineID = @id";

                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int productLineId = reader.GetInt32(0);
                        int quantity = reader.GetInt32(1);
                        int productId = reader.GetInt32(2);
                        int? basketId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3);
                        int? orderNumber = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4);

                        return new ProductLine(productLineId, productId, quantity, basketId, orderNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.Close();
            }

            return null;
        }

        public bool UpdateProductLine(ProductLine productLine)
        {
            try
            {
                _connection.Open();

                string query = "UPDATE ProductLine SET Quantity = @quantity, ProductID = @productId, BasketID = @basketId, OrderNumber = @orderNumber WHERE ProductLineID = @id";

                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@quantity", productLine.Quantity);
                    command.Parameters.AddWithValue("@productId", productLine.ProductID);
                    command.Parameters.AddWithValue("@basketId", (object)productLine.BasketID ?? DBNull.Value);
                    command.Parameters.AddWithValue("@orderNumber", (object)productLine.OrderNumber ?? DBNull.Value);
                    command.Parameters.AddWithValue("@id", productLine.Id);

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.Close();
            }

            return false;
        }
    }
}


