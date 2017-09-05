using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simchas.Data
{
    public class SimchasDB
    {
        private string _connectionString;
        public SimchasDB(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IEnumerable<Simcha> GetAllSimchas()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "Select * FROM Simchas ORDER BY Id DESC";
                connection.Open();
                List<Simcha> result = new List<Simcha>();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Simcha simcha = new Simcha
                    {
                        Id = (int)reader["Id"],
                        Date = (DateTime)reader["Date"],
                        Name = (string)reader["Name"]
                    };
                    result.Add(simcha);
                }
                return result;
            }
        }
        public void AddSimcha(Simcha simcha)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Simchas VALUES (@name, @date)";
                command.Parameters.AddWithValue("@name", simcha.Name);
                command.Parameters.AddWithValue("@date", simcha.Date);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public IEnumerable<Contributor> GetContributorsForSimcha(int SimchaId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM SimchaContributor sc JOIN Contributors c ON c.Id = sc.ContributorId WHERE sc.SimchaId = @simchaId";
                command.Parameters.AddWithValue("@simchaId", SimchaId);
                connection.Open();
                List<Contributor> result = new List<Contributor>();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Contributor contributor = new Contributor
                    {
                        Id = (int)reader["Id"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        PhoneNumber = (string)reader["PhoneNumber"],
                        DateCreated = (DateTime)reader["DateCreated"],
                        AlwaysInclude = (bool)reader["AlwaysInclude"]
                    };
                    result.Add(contributor);
                }
                return result;
            }
        }
        public int GetTotalDeposits(int SimchaId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT SUM(sc.Amount) FROM SimchaContributor sc JOIN Simchas s ON s.Id = sc.SimchaId WHERE s.Id = @simchaId";
                command.Parameters.AddWithValue("@simchaId", SimchaId);
                connection.Open();
                if (command.ExecuteScalar() != DBNull.Value)
                {
                    return (int)(decimal)command.ExecuteScalar();
                }
                return 0;
            }
        }
        public Simcha GetById(int SimchaId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "Select * FROM Simchas WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", SimchaId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    return null;
                }
                Simcha simcha = new Simcha
                {
                    Id = (int)reader["Id"],
                    Date = (DateTime)reader["Date"],
                    Name = (string)reader["Name"]
                };
                return simcha;
            }
        }
    }
}
