using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace windTaskBase
{
    internal class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private User CreateObject(MySqlDataReader reader)
        {
            return new User
            {
                Id = reader.GetInt32("id"),
                Name = reader.GetString("name"),
                Email = reader.GetString("email"),
                CreatedAt = reader.GetDateTime("createdAt"),
                UpdatedAt = reader.GetDateTime("updatedAt"),
                Birthday = reader.GetDateTime("birthDay")
            };
        }

        public int Create(string Name, string Email, string Password, DateTime Birthday)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var command = new MySqlCommand("INSERT INTO users (name, email, password, birthDay) VALUES (@Name, @Email, @Password, @Birthday); SELECT LAST_INSERT_ID();", connection);

                command.Parameters.AddWithValue("@Name", Name);
                command.Parameters.AddWithValue("@Email", Email);
                command.Parameters.AddWithValue("@Password", Password);
                command.Parameters.AddWithValue("@Birthday", Birthday);

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public List<User> GetAll()
        {
            var users = new List<User>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var command = new MySqlCommand("SELECT id, name, email, createdAt, updatedAt, birthDay FROM users", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(CreateObject(reader));
                    }
                }
            }

            return users;
        }
    }
}
