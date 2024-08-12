using System.Data;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace UserApi.Repository
{
    public interface IUserRepository
    {
        Task<int?> LoginAsync(string username, string password);
        Task<(string Name, string PhoneNumber)> GetUserDetailsAsync(int userId);
    }
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<(string Name, string PhoneNumber)> GetUserDetailsAsync(int userId)
        {
            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand("GetUserDetails", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("p_userId", userId);
            command.Parameters.Add("p_name", MySqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            command.Parameters.Add("p_phoneNumber", MySqlDbType.VarChar, 15).Direction = ParameterDirection.Output;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            var name = command.Parameters["p_name"].Value.ToString();
            var phoneNumber = command.Parameters["p_phoneNumber"].Value.ToString();

            return (name, phoneNumber);
        }

        public async Task<int?> LoginAsync(string username, string password)
        {
            using var connection = new MySqlConnection(_connectionString);
            using var command = new MySqlCommand("UserLogin", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("p_username", username);
            command.Parameters.AddWithValue("p_password", password);
            command.Parameters.Add("p_userId", MySqlDbType.Int32).Direction = ParameterDirection.Output;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            var userId = command.Parameters["p_userId"].Value;
            return userId != DBNull.Value ? Convert.ToInt32(userId) : (int?)null;
        }
    }
}
