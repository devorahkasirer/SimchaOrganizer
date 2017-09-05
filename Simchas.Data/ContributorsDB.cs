using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simchas.Data
{
    public class ContributorsDB
    {
        private string _connectionString;
        public ContributorsDB(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IEnumerable<Contributor> GetAllContributors()
        {         
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "Select * FROM Contributors";
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
        public int AddContributor(Contributor contributor)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Contributors VALUES (@firstName, @lastName, @phoneNumber, @dateCreated, @alwaysInclude); SELECT @@IDENTITY";
                command.Parameters.AddWithValue("@firstName", contributor.FirstName);
                command.Parameters.AddWithValue("@lastName", contributor.LastName);
                command.Parameters.AddWithValue("@phoneNumber", contributor.PhoneNumber);
                command.Parameters.AddWithValue("@dateCreated", DateTime.Now);
                command.Parameters.AddWithValue("@alwaysInclude", contributor.AlwaysInclude);
                connection.Open();
                return (int)(decimal)command.ExecuteScalar();
            }
        }
        public IEnumerable<Deposit> GetDepositsForContributor(int ContributorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "Select * FROM Deposits WHERE ContributorId = @contributorId";
                command.Parameters.AddWithValue("@contributorId", ContributorId);
                connection.Open();
                List<Deposit> result = new List<Deposit>();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Deposit deposit = new Deposit
                    {
                        Id = (int)reader["Id"],
                        Date = (DateTime)reader["Date"],
                        Amount = (int)(decimal)reader["Amount"],
                        ContributorId = (int)reader["ContributorId"]
                    };
                    result.Add(deposit);
                }
                return result;
            }
        }
        public Contributor GetById(int ContributorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "Select * FROM Contributors WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", ContributorId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    return null;
                }
                Contributor contributor = new Contributor
                {
                    Id = (int)reader["Id"],
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    DateCreated = (DateTime)reader["DateCreated"],
                    AlwaysInclude = (bool)reader["AlwaysInclude"]
                };
                return contributor;
            }
        }
        public void AddDeposit(int deposit, int contributorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Deposits VALUES (@date, @amount, @contributorId)";
                command.Parameters.AddWithValue("@date", DateTime.Now);
                command.Parameters.AddWithValue("@amount", deposit);
                command.Parameters.AddWithValue("@contributorId", contributorId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public int GetTotalDeposits(int contributorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT SUM(sc.Amount) FROM SimchaContributor sc WHERE sc.ContributorId = @contributorId";
                command.Parameters.AddWithValue("@contributorId", contributorId);
                connection.Open();
                if (command.ExecuteScalar() != DBNull.Value)
                {
                    return (int)(decimal)command.ExecuteScalar();
                }
                return 0;
            }
        }
        public int GetBalance(int contributorId)
        {
            int spent = GetTotalDeposits(contributorId);
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT SUM(d.Amount) FROM Deposits d WHERE d.ContributorId = @contributorId";
                command.Parameters.AddWithValue("@contributorId", contributorId);
                connection.Open();
                int total = 0;
                if (command.ExecuteScalar() != DBNull.Value)
                {
                    total = (int)(decimal)command.ExecuteScalar();
                }
                return total - spent;
            }
        }
        public int TotalNumberContributors()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM Contributors";
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }
        public int NumberContributorsForSimcha(int SimchaId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM Contributors c JOIN SimchaContributor sc ON c.Id = sc.ContributorId JOIN Simchas s ON sc.SimchaId = s.Id WHERE s.Id = @Id";
                command.Parameters.AddWithValue("@Id", SimchaId);
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }
        public IEnumerable<Contribution> GetContributionsForContributor(int ContributorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Contributors c JOIN SimchaContributor sc ON sc.ContributorId = c.id JOIN Simchas s ON s.id = sc.SimchaId WHERE c.id = @Id";
                command.Parameters.AddWithValue("@Id", ContributorId);
                connection.Open();
                List<Contribution> result = new List<Contribution>();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Contribution c = new Contribution
                    {
                        Amount = (int)(decimal)reader["Amount"],
                        ContributorId = (int)reader["ContributorId"],
                        SimchaId = (int)reader["SimchaId"],
                        Name = (string)reader["Name"],
                        Date = (DateTime)reader["Date"]
                    };
                    result.Add(c);
                }
                return result;
            }
        }
        public void UpdateContribution(List<Contributing> contributing, int SimchaId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM SimchaContributor WHERE SimchaId = @Id";
                command.Parameters.AddWithValue("@Id", SimchaId);
                connection.Open();
                command.ExecuteNonQuery();

                command.Parameters.Clear();
                foreach (Contributing c in contributing)
                {
                    if (c.Include)
                    {
                        command.Parameters.Clear();
                        command.CommandText = "INSERT INTO SimchaContributor VALUES (@SimchaId, @ContributorId, @Amount)";
                        command.Parameters.AddWithValue("@SimchaId", SimchaId);
                        command.Parameters.AddWithValue("@ContributorId", c.ContributorId);
                        command.Parameters.AddWithValue("@Amount", c.Amount);
                        command.ExecuteNonQuery();
                    }
                }
            } 
        }
        public bool ContributedAlready(int SimchaId, int ContributorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM SimchaContributor WHERE SimchaId = @sId AND ContributorId = @cId";
                command.Parameters.AddWithValue("@sId", SimchaId);
                command.Parameters.AddWithValue("@cId", ContributorId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                return reader.Read();
            }
        }
        public int ContributedAmount(int SimchaId, int ContributorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM SimchaContributor WHERE SimchaId = @sId AND ContributorId = @cId";
                command.Parameters.AddWithValue("@sId", SimchaId);
                command.Parameters.AddWithValue("@cId", ContributorId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    return 0;
                }
                else
                {
                    return (int)(decimal)reader["Amount"];
                }
            }
        }
    }
}
