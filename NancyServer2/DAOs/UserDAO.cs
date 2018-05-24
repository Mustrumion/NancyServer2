using NancyServer2.Objects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NancyServer2.DAOs
{
    public class UserDAO
    {
        private SqlConnection conn;

        public UserDAO()
        {
            this.conn = new SqlConnection();
            conn.ConnectionString =
            $"Data Source={ConnectionData.Server};" +
            $"Initial Catalog={ConnectionData.Database};" +
            $"{ConnectionData.AuthenticationString};";
            conn.Open();
        }

        public User GetUser(int userID)
        {
            User user = null;
            SqlCommand comm = new SqlCommand()
            {
                CommandText = "SELECT * FROM users WHERE id = @id",
                CommandType = System.Data.CommandType.Text,
                CommandTimeout = 2000,
                Connection = this.conn
            };
            comm.Parameters.AddWithNullableValue("id", userID);
            SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                user = new User()
                {
                    ID = reader.GetConverted<int>("id"),
                    Email = reader.GetConverted<string>("email")
                };
            }
            reader.Close();
            return user;
        }

        public User GetUserByEmail(string email)
        {
            User user = null;
            SqlCommand comm = new SqlCommand()
            {
                CommandText = "SELECT * FROM users WHERE email = @email",
                CommandType = System.Data.CommandType.Text,
                CommandTimeout = 2000,
                Connection = this.conn
            };
            comm.Parameters.AddWithNullableValue("email", email);
            SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                user = new User()
                {
                    ID = reader.GetConverted<int>("id"),
                    Email = reader.GetConverted<string>("email")
                };
            }
            reader.Close();
            return user;
        }

        public bool UserExists(User user)
        {
            bool result = false;
            SqlCommand comm = new SqlCommand()
            {
                CommandText = "SELECT * FROM users WHERE email = @email",
                CommandType = System.Data.CommandType.Text,
                CommandTimeout = 2000,
                Connection = this.conn
            };
            comm.Parameters.AddWithNullableValue("email", user.Email);
            SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                result = true;
            }
            reader.Close();
            return result;
        }

        public bool Register(User user)
        {
            SqlCommand comm = new SqlCommand()
            {
                CommandText = "BEGIN TRANSACTION\n"
                        + "DECLARE @id AS INT = (SELECT id FROM users WHERE email = @email)\n"
                        + "IF @id IS NULL BEGIN\n"
                        + "INSERT INTO dbo.Users(email, password) VALUES(@email, @password)\n"
                        + "END\n"
                        + "COMMIT",
                CommandType = System.Data.CommandType.Text,
                CommandTimeout = 2000,
                Connection = this.conn
            };
            comm.Parameters.AddWithNullableValue("email", user.Email);
            byte[] password = null;
            using (SHA512 shaM = new SHA512Managed())
            {
                password = shaM.ComputeHash(Encoding.ASCII.GetBytes(user.Password));
            }
            comm.Parameters.AddWithNullableValue("password", password);
            int rowcount = comm.ExecuteNonQuery();
            if(rowcount == 0)
            {
                return false;
            }
            return true;
        }

        public Guid? LogIn(User user)
        {
            Guid? result = null;
            SqlCommand comm = new SqlCommand()
            {
                CommandText = "BEGIN TRANSACTION\n"
                        + "DECLARE @id AS INT = (SELECT id FROM users WHERE email = @email AND password = @password)\n"
                        + "IF @id IS NOT NULL BEGIN\n"
                        + "DECLARE @token AS UNIQUEIDENTIFIER = NEWID()\n"
                        + "INSERT INTO dbo.Tokens([guid], userID, expiration)\n"
                        + "VALUES(@token, @id, DATEADD(MINUTE, 60, CURRENT_TIMESTAMP))\n"
                        + "SELECT @token as token END\n"
                        + "COMMIT",
                CommandType = System.Data.CommandType.Text,
                CommandTimeout = 2000,
                Connection = this.conn
            };
            comm.Parameters.AddWithNullableValue("email", user.Email);
            byte[] password = null;
            using (SHA512 shaM = new SHA512Managed())
            {
                password = shaM.ComputeHash(Encoding.ASCII.GetBytes(user.Password));
            }
            comm.Parameters.AddWithNullableValue("password", password);
            SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                result = reader.GetConverted<Guid?>("token");
            }
            reader.Close();
            return result;
        }
    }
}
