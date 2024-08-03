using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DAL.DAO;
using DAL.Models;

namespace DAL.DB
{
    public class BasketDB : BasketDAO
    {
        public string ConnectionString { get; private set; }
        private SqlConnection _connection;

        public BasketDB(string connectionString)
        {
            ConnectionString = connectionString;
            _connection = new SqlConnection(connectionString);
        }

        public void AddBasket(Basket basket)
        {
            try
            {
                _connection.Open();

                string query = "INSERT INTO Basket (CreatedAt) VALUES (@CreatedAt); SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@CreatedAt", basket.CreatedAt);

                    basket.Id = Convert.ToInt32(command.ExecuteScalar());
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

        public bool RemoveBasket(int basketId)
        {
            try
            {
                _connection.Open();

                string query = "DELETE FROM Basket WHERE BasketID = @BasketID";

                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@BasketID", basketId);

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

        public Basket? GetBasketById(int basketId)
        {
            try
            {
                _connection.Open();

                string query = "SELECT BasketID, CreatedAt FROM Basket WHERE BasketID = @BasketID";

                using (var command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@BasketID", basketId);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        DateTime createdAt = reader.GetDateTime(1);

                        return new Basket(id, createdAt);
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

        public IEnumerable<Basket> GetAllBaskets()
        {
            List<Basket> baskets = new List<Basket>();

            try
            {
                _connection.Open();

                string query = "SELECT BasketID, CreatedAt FROM Basket";

                using (var command = new SqlCommand(query, _connection))
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        DateTime createdAt = reader.GetDateTime(1);

                        baskets.Add(new Basket(id, createdAt));
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

            return baskets;
        }
    }
}





