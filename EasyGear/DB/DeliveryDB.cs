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
    public class DeliveryDB : DeliveryDAO
    {
        public string ConnectionString { get; private set; }
        private SqlConnection _connection;

        public DeliveryDB(string connectionString)
        {
            ConnectionString = connectionString;
            _connection = new SqlConnection(connectionString);
        }

        public Delivery? GetOrderById(int orderNumber)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT OrderNumber, DeliveredTo, DeliveryDate FROM Delivery WHERE OrderNumber = @OrderNumber;", connection))
                {
                    command.Parameters.AddWithValue("@OrderNumber", orderNumber);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Delivery
                            {
                                OrderNumber = reader.GetInt32(0),
                                DeliveredTo = reader.GetString(1),
                                DeliveryDate = reader.GetDateTime(2)
                            };
                        }
                    }
                }
            }

            return null;
        }


        public void AddOrder(Delivery delivery)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO Delivery (DeliveredTo, DeliveryDate) VALUES (@DeliveredTo, @DeliveryDate); SELECT SCOPE_IDENTITY();", connection))
                {
                    command.Parameters.AddWithValue("@DeliveredTo", delivery.DeliveredTo);
                    command.Parameters.AddWithValue("@DeliveryDate", delivery.DeliveryDate);
                    delivery.OrderNumber = Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public bool DeleteOrder(int orderNumber)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM Delivery WHERE OrderNumber = @OrderNumber;", connection))
                {
                    command.Parameters.AddWithValue("@OrderNumber", orderNumber);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public IEnumerable<Delivery> GetAllOrders()
        {
            var deliveries = new List<Delivery>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT OrderNumber, DeliveryDate, DeliveredTo FROM Delivery;", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            deliveries.Add(new Delivery
                            {
                                OrderNumber = reader.GetInt32(0),
                                DeliveryDate = reader.GetDateTime(1),
                                DeliveredTo = reader.GetString(2)
                            });
                        }
                    }
                }
            }

            return deliveries;
        }


        public bool UpdateOrder(Delivery delivery)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE Delivery SET DeliveredTo = @DeliveredTo, DeliveryDate = @DeliveryDate WHERE OrderNumber = @OrderNumber;", connection))
                {
                    command.Parameters.AddWithValue("@OrderNumber", delivery.OrderNumber);
                    command.Parameters.AddWithValue("@DeliveredTo", delivery.DeliveredTo);
                    command.Parameters.AddWithValue("@DeliveryDate", delivery.DeliveryDate);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}