using DAL.DAO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DB
{
    public class AccountDB: AccountDAO
    {
        public string ConnectionString { get; private set; }
        private SqlConnection _connection;

        public AccountDB(string connectionString)
        {
            ConnectionString = connectionString;
            _connection = new SqlConnection(connectionString);
        }

        public Account? Login(string email, string password)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Account WHERE Email = @Email AND PasswordWithHash = @PasswordWithHash", connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PasswordWithHash", password);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Account(
                            Convert.ToInt32(reader["AccountID"]),
                            reader["UserName"].ToString(),
                            reader["Email"].ToString(),
                            reader["PasswordWithHash"].ToString()
                        );
                    }
                }
            }
            return null;
        }

        public Account? GetById(int accountID)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Account WHERE AccountID = @AccountID", connection);
                command.Parameters.AddWithValue("@AccountID", accountID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Account(
                            Convert.ToInt32(reader["AccountID"]),
                            reader["UserName"].ToString(),
                            reader["Email"].ToString(),
                            reader["PasswordWithHash"].ToString()
                        );
                    }
                }
            }
            return null;
        }


        public int Add(Account account)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("INSERT INTO Account (UserName, Email, PasswordWithHash) VALUES (@UserName, @Email, @PasswordWithHash); SELECT SCOPE_IDENTITY();", connection);
                command.Parameters.AddWithValue("@UserName", account.UserName);
                command.Parameters.AddWithValue("@Email", account.Email);
                command.Parameters.AddWithValue("@PasswordWithHash", account.Password);

                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
}
