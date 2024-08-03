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

        public void AddOrder(Delivery delivery)
        {
            try
            {
                _connection.Open();

                string query = "INSERT INTO Delivery (DeliveredTo, DeliveryDate, AccountID) VALUES (@DeliveredTo, @DeliveryDate, @AccountID); SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@DeliveredTo", delivery.DeliveredTo);
                    command.Parameters.AddWithValue("@DeliveryDate", delivery.DeliveryDate);
                    command.Parameters.AddWithValue("@AccountID", delivery.AccountID);

                    delivery.Id = Convert.ToInt32(command.ExecuteScalar());
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

                string query = "SELECT OrderNumber, DeliveredTo, DeliveryDate, AccountID FROM Delivery";

                using (var command = new SqlCommand(query, _connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string deliveredTo = reader.GetString(1);
                        DateTime deliveryDate = reader.GetDateTime(2);
                        int accountId = reader.GetInt32(3);

                        deliveries.Add(new Delivery(id, deliveredTo, deliveryDate, accountId));
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

                string query = "SELECT OrderNumber, DeliveredTo, DeliveryDate, AccountID FROM Delivery WHERE OrderNumber = @OrderNumber";

                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@OrderNumber", id);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int orderId = reader.GetInt32(0);
                        string deliveredTo = reader.GetString(1);
                        DateTime deliveryDate = reader.GetDateTime(2);
                        int accountId = reader.GetInt32(3);

                        return new Delivery(orderId, deliveredTo, deliveryDate, accountId);
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

                string query = "UPDATE Delivery SET DeliveredTo = @DeliveredTo, DeliveryDate = @DeliveryDate, AccountID = @AccountID WHERE OrderNumber = @OrderNumber";

                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@DeliveredTo", delivery.DeliveredTo);
                    command.Parameters.AddWithValue("@DeliveryDate", delivery.DeliveryDate);
                    command.Parameters.AddWithValue("@AccountID", delivery.AccountID);
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




