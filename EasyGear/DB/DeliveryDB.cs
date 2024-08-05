using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DAL.DAO;
using DAL.Models;

namespace DAL.DB
{
    public class DeliveryDB : DeliveryDAO
    {
        public string ConnectionString { get; private set; }
        private SqlConnection _connection;

        public DeliveryDB(string connectionString)
        {
            ConnectionString = connectionString;
            _connection = new SqlConnection(connectionString);
        }

        public int AddOrder(Delivery delivery)
        {
            SqlTransaction transaction = null;
            try
            {
                _connection.Open();
                transaction = _connection.BeginTransaction();

                string query = "INSERT INTO Delivery (DeliveredTo, DeliveryDate) VALUES (@DeliveredTo, @DeliveryDate); SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, _connection, transaction))
                {
                    command.Parameters.AddWithValue("@DeliveredTo", delivery.DeliveredTo);
                    command.Parameters.AddWithValue("@DeliveryDate", delivery.DeliveryDate);

                    // Get the ID of the newly inserted delivery
                    int deliveryId = Convert.ToInt32(command.ExecuteScalar());

                    // Commit transaction
                    transaction.Commit();

                    return deliveryId;
                }
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

                Console.WriteLine($"Error inserting Delivery: {ex.Message}");
                // Consider logging the exception to a file or monitoring system
                return -1; // Indicate failure
            }
            finally
            {
                if (_connection.State == System.Data.ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
        }



        public bool DeleteOrder(int id)
        {
            try
            {
                _connection.Open();

                string query = "DELETE FROM Delivery WHERE OrderNumber = @OrderNumber";

                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@OrderNumber", id);

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                _connection.Close();
            }
        }

        public IEnumerable<Delivery> GetAllOrders()
        {
            List<Delivery> deliveries = new List<Delivery>();

            try
            {
                _connection.Open();

                string query = "SELECT OrderNumber, DeliveredTo, DeliveryDate FROM Delivery";

                using (var command = new SqlCommand(query, _connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string deliveredTo = reader.GetString(1);
                        DateTime deliveryDate = reader.GetDateTime(2);

                        deliveries.Add(new Delivery(id, deliveredTo, deliveryDate));
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

            return deliveries;
        }

        public Delivery? GetOrderById(int id)
        {
            try
            {
                _connection.Open();

                string query = "SELECT OrderNumber, DeliveredTo, DeliveryDate FROM Delivery WHERE OrderNumber = @OrderNumber";

                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@OrderNumber", id);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int orderId = reader.GetInt32(0);
                        string deliveredTo = reader.GetString(1);
                        DateTime deliveryDate = reader.GetDateTime(2);

                        return new Delivery(orderId, deliveredTo, deliveryDate);
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

        public bool UpdateOrder(Delivery delivery)
        {
            try
            {
                _connection.Open();

                string query = "UPDATE Delivery SET DeliveredTo = @DeliveredTo, DeliveryDate = @DeliveryDate WHERE OrderNumber = @OrderNumber";

                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@DeliveredTo", delivery.DeliveredTo);
                    command.Parameters.AddWithValue("@DeliveryDate", delivery.DeliveryDate);
                    command.Parameters.AddWithValue("@OrderNumber", delivery.Id);

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}




