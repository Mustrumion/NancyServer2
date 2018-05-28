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
    public class UserProfileDAO : SQLServerBaseDAO
    {
        public UserProfileDAO() : base()
        {
        }

        public UserProfile GetUserProfile(int userID)
        {
            UserProfile result = null;
            SqlCommand comm = new SqlCommand()
            {
                CommandText = "SELECT * FROM userProfiles WHERE userID = @userID",
                CommandType = System.Data.CommandType.Text,
                CommandTimeout = 2000,
                Connection = this.conn
            };
            comm.Parameters.AddWithNullableValue("userID", userID);
            SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                result = new UserProfile()
                {
                    Name = reader.GetConverted<string>("name"),
                    NameVisible = reader.GetConverted<bool>("nameVisible"),
                    Surname = reader.GetConverted<string>("surname"),
                    SurnameVisible = reader.GetConverted<bool>("surnameVisible"),
                    Description = reader.GetConverted<string>("description"),
                    DescriptionVisible = reader.GetConverted<bool>("descriptionVisible"),
                    Interests = reader.GetConverted<string>("interests"),
                    InterestsVisible = reader.GetConverted<bool>("interestsVisible"),
                    NickVisible = reader.GetConverted<bool>("nickVisible"),
                    Gender = reader.GetConverted<string>("gender"),
                    GenderVisible = reader.GetConverted<bool>("genderVisible"),
                    Birth = reader.GetConverted<DateTime>("name"),
                    AgeVisible = reader.GetConverted<bool>("ageVisible")
                };
            }
            reader.Close();
            return result;
        }
    }
}
