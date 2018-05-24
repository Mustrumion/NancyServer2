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
                CommandText =
                          "DECLARE @id AS INT = (SELECT id FROM users WHERE email = @email)\n"
                        + "IF @id IS NULL BEGIN\n"
                        + "INSERT INTO dbo.Users(email, password) VALUES(@email, @password)\n"
                        + "SELECT 0 AS error\n"
                        + "END ELSE BEGIN\n"
                        + "SELECT 1 AS error\n"
                        + "END\n",
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
            int error = 2;
            if (reader.Read())
            {
                error = reader.GetConverted<int>("error");
            }
            reader.Close();
            if (error == 0)
            {
                return true;
            }
            return false;
        }

        public Token LogIn(User user)
        {
            Token result = null;
            SqlCommand comm = new SqlCommand()
            {
                CommandText = 
                          "DECLARE @id AS INT = (SELECT id FROM users WHERE email = @email AND password = @password)\n"
                        + "IF @id IS NOT NULL BEGIN\n"
                        + "DECLARE @token AS UNIQUEIDENTIFIER = NEWID()\n"
                        + "DECLARE @expiration AS DATETIME = DATEADD(MINUTE, 60, CURRENT_TIMESTAMP)\n"
                        + "INSERT INTO dbo.Tokens([guid], userID, expiration)\n"
                        + "VALUES(@token, @id, @expiration)\n"
                        + "SELECT @token AS token, @expiration AS expiration, @id AS userID END\n",
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
                result = new Token();
                result.SessionID = reader.GetConverted<Guid>("token");
                result.Expiration = reader.GetConverted<DateTime>("expiration");
                result.User = new User()
                {
                    ID = reader.GetConverted<int>("userID"),
                    Email = user.Email
                };
            }
            reader.Close();
            return result;
        }

        public string IsTokenValid(Token token)
        {
            string error = null;
            SqlCommand comm = new SqlCommand()
            {
                CommandText =
                          "DECLARE @expiration AS DATETIME\n"
                        + "DECLARE @foundid AS INT\n"
                        + "SELECT @foundid = userID, @expiration = expiration FROM dbo.Tokens WHERE guid = @token \n"
                        + "IF @foundid IS NULL OR @expiration IS NULL BEGIN\n"
                        + "SELECT 3 AS error RETURN END\n"
                        + "IF @foundid <> @userid BEGIN\n"
                        + "SELECT 1 AS error RETURN END\n"
                        + "IF @expiration < CURRENT_TIMESTAMP BEGIN\n"
                        + "SELECT 2 AS error RETURN END\n"
                        + "SELECT 0 AS error\n",
                CommandType = System.Data.CommandType.Text,
                CommandTimeout = 2000,
                Connection = this.conn
            };
            comm.Parameters.AddWithNullableValue("@userid", token.User.ID);
            comm.Parameters.AddWithNullableValue("@token", token.SessionID);
            SqlDataReader reader = comm.ExecuteReader();
            int errorCode = 3;
            if (reader.Read())
            {
                errorCode = reader.GetConverted<int>("error");
            }
            reader.Close();
            if (errorCode == 1)
            {
                error = "User doesn't match token.";
            }
            if (errorCode == 2)
            {
                error = "Token expired.";
            }
            if (errorCode == 3)
            {
                error = "Token doesn't exist.";
            }
            return error;
        }


        public string IsTokenValid(Guid token)
        {
            string error = null;
            SqlCommand comm = new SqlCommand()
            {
                CommandText =
                          "DECLARE @expiration AS DATETIME\n"
                        + "DECLARE @foundid AS INT\n"
                        + "SELECT @foundid = userID, @expiration = expiration FROM dbo.Tokens WHERE guid = @token \n"
                        + "IF @foundid IS NULL OR @expiration IS NULL BEGIN\n"
                        + "SELECT 3 AS error RETURN END\n"
                        + "IF @expiration < CURRENT_TIMESTAMP BEGIN\n"
                        + "SELECT 2 AS error RETURN END\n"
                        + "SELECT 0 AS error\n",
                CommandType = System.Data.CommandType.Text,
                CommandTimeout = 2000,
                Connection = this.conn
            };
            comm.Parameters.AddWithNullableValue("@token", token);
            SqlDataReader reader = comm.ExecuteReader();
            int errorCode = 3;
            if (reader.Read())
            {
                errorCode = reader.GetConverted<int>("error");
            }
            reader.Close();
            if (errorCode == 1)
            {
                error = "User doesn't match token.";
            }
            if (errorCode == 2)
            {
                error = "Token expired.";
            }
            if (errorCode == 3)
            {
                error = "Token doesn't exist.";
            }
            return error;
        }
    }
}
