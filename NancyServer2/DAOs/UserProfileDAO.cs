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
                    Nick = reader.GetConverted<string>("nick"),
                    NickVisible = reader.GetConverted<bool>("nickVisible"),
                    Gender = reader.GetConverted<string>("gender"),
                    GenderVisible = reader.GetConverted<bool>("genderVisible"),
                    Birth = reader.GetConverted<DateTime>("born"),
                    AgeVisible = reader.GetConverted<bool>("ageVisible")
                };
            }
            reader.Close();
            return result;
        }

        public void SaveUserProfile(UserProfile profile)
        {
            SqlCommand comm = new SqlCommand()
            {
                CommandText =
                          "INSERT INTO UserProfiles(name, nameVisible, surname, surnameVisible, description, descriptionVisible,\n"
                        + "interests, interestsVisible, nick, nickVisible, gender, genderVisible, born, ageVisible)\n"
                        + "VALUES(@name, @nameVis, @surname, @surnameVis, @description, @descriptionVis,\n"
                        + "@interests, @interestsVis, @nick, @nickVis, @surname, @surnameVis, @description, @descritionVis)\n"
                        + "WHERE userID = @userID",
                CommandType = System.Data.CommandType.Text,
                CommandTimeout = 2000,
                Connection = this.conn
            };
            comm.Parameters.AddWithNullableValue("userID", profile.UserID);
            comm.Parameters.AddWithNullableValue("name", profile.Name);
            comm.Parameters.AddWithNullableValue("surname", profile.Surname);
            comm.Parameters.AddWithNullableValue("nick", profile.Nick);
            comm.Parameters.AddWithNullableValue("gender", profile.Gender);
            comm.Parameters.AddWithNullableValue("born", profile.Birth);
            comm.Parameters.AddWithNullableValue("description", profile.Description);
            comm.Parameters.AddWithNullableValue("interests", profile.Interests);
            comm.Parameters.AddWithNullableValue("nameVis", profile.NameVisible);
            comm.Parameters.AddWithNullableValue("surnameVis", profile.SurnameVisible);
            comm.Parameters.AddWithNullableValue("genderVis", profile.GenderVisible);
            comm.Parameters.AddWithNullableValue("ageVis", profile.AgeVisible);
            comm.Parameters.AddWithNullableValue("interestsVis", profile.InterestsVisible);
            comm.Parameters.AddWithNullableValue("descriptionVis", profile.DescriptionVisible);
            comm.Parameters.AddWithNullableValue("nickVis", profile.NickVisible);
            comm.ExecuteNonQuery();
        }
    }
}
