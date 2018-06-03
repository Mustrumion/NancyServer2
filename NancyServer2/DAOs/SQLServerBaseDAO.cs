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
    public class SQLServerBaseDAO
    {
        protected SqlConnection conn;

        public enum TokenState
        {
            DoesNotExist = 3,
            Expired = 2,
            WrongUser = 1,
            OK = 0
        }

        public SQLServerBaseDAO()
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

        public TokenState GetTokenState(Token token)
        {
            TokenState error = TokenState.DoesNotExist;
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
            if (reader.Read())
            {
                error = (TokenState)(reader.GetConverted<int>("error"));

            }
            reader.Close();
            return error;
        }


        public TokenState GetTokenState(Guid token)
        {
            TokenState error = TokenState.DoesNotExist;
            SqlCommand comm = new SqlCommand()
            {
                CommandText =
                          "DECLARE @expiration AS DATETIME\n"
                        + "DECLARE @foundid AS INT\n"
                        + "SELECT @foundid = userID, @expiration = expiration FROM dbo.Tokens WHERE guid = @token \n"
                        + "IF @foundid IS NULL OR @expiration IS NULL BEGIN\n"
                        + "SELECT @foundid as user, 3 AS error RETURN END\n"
                        + "IF @expiration < CURRENT_TIMESTAMP BEGIN\n"
                        + "SELECT @foundid as user, 2 AS error RETURN END\n"
                        + "SELECT @foundid as user, 0 AS error\n",
                CommandType = System.Data.CommandType.Text,
                CommandTimeout = 2000,
                Connection = this.conn
            };
            comm.Parameters.AddWithNullableValue("@token", token);
            SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                error = (TokenState)(reader.GetConverted<int>("error"));
            }
            reader.Close();
            return error;
        }
    }
}
