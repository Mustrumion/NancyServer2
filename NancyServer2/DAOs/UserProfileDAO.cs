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
                    Gender = reader.GetConverted<string>("gender"),
                    GenderVisible = reader.GetConverted<bool>("genderVisible"),
                    Born = reader.GetConverted<DateTime>("born"),
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
                          "UPDATE dbo.UserProfiles SET name = @name, nameVisible = @nameVis, surname = @surname, surnameVisible = @surnameVis,\n" +
                          "description = @description, descriptionVisible = @descriptionVis, interests = @interests, interestsVisible = @interestsVis,\n" +
                          "nick = @nick, gender = @gender, genderVisible = @genderVis, born = @born, ageVisible = @ageVis\n" +
                          "WHERE userID = @userID\n" +
                          "IF @@ROWCOUNT = 0\n" +
                          "INSERT INTO UserProfiles(userId, name, nameVisible, surname, surnameVisible, description, descriptionVisible,\n" +
                          "interests, interestsVisible, nick, gender, genderVisible, born, ageVisible)\n" +
                          "VALUES(@userId, @name, @nameVis, @surname, @surnameVis, @description, @descriptionVis,\n" + 
                          "@interests, @interestsVis, @nick, @gender, @genderVis, @born, @ageVis)\n",
                CommandType = System.Data.CommandType.Text,
                CommandTimeout = 2000,
                Connection = this.conn
            };
            comm.Parameters.AddWithNullableValue("userID", profile.UserID);
            comm.Parameters.AddWithNullableValue("name", profile.Name);
            comm.Parameters.AddWithNullableValue("surname", profile.Surname);
            comm.Parameters.AddWithNullableValue("nick", profile.Nick);
            comm.Parameters.AddWithNullableValue("gender", profile.Gender);
            if(profile.Born.Year < 1800 || profile.Born.Year > 9000)
            {
                comm.Parameters.AddWithNullableValue("born", DBNull.Value);
            }
            else
            {
                comm.Parameters.AddWithNullableValue("born", profile.Born);
            }
            comm.Parameters.AddWithNullableValue("description", profile.Description);
            comm.Parameters.AddWithNullableValue("interests", profile.Interests);
            comm.Parameters.AddWithNullableValue("nameVis", profile.NameVisible);
            comm.Parameters.AddWithNullableValue("surnameVis", profile.SurnameVisible);
            comm.Parameters.AddWithNullableValue("genderVis", profile.GenderVisible);
            comm.Parameters.AddWithNullableValue("ageVis", profile.AgeVisible);
            comm.Parameters.AddWithNullableValue("interestsVis", profile.InterestsVisible);
            comm.Parameters.AddWithNullableValue("descriptionVis", profile.DescriptionVisible);
            comm.ExecuteNonQuery();
        }

        public void SaveUserProfilePhoto(UserPhoto photo)
        {
            SqlCommand comm = new SqlCommand()
            {
                CommandText =
                          "UPDATE dbo.Photos SET photo = @photo\n" +
                          "WHERE guid = (SELECT photoGuid FROM dbo.UserProfiles WHERE userID = @userID)\n" +
                          "IF @@ROWCOUNT = 0 BEGIN\n" +
                          "DECLARE @newguid AS UNIQUEIDENTIFIER = NEWID()\n" +
                          "INSERT INTO dbo.Photos(guid, photo) VALUES(@newguid, @photo)\n" +
                          "UPDATE dbo.UserProfiles SET photoGuid = @newguid WHERE userID = @userID END",
                CommandType = System.Data.CommandType.Text,
                CommandTimeout = 2000,
                Connection = this.conn
            };
            comm.Parameters.AddWithNullableValue("userID", photo.UserID);
            comm.Parameters.AddWithNullableValue("photo", Convert.FromBase64String(photo.PhotoBase64));
            comm.ExecuteNonQuery();
        }

        public UserPhoto GetUserProfilePhoto(int userID)
        {
            UserPhoto result = null;
            SqlCommand comm = new SqlCommand()
            {
                CommandText =
                          "SELECT id, photo FROM dbo.UserProfiles prof " +
                          "LEFT JOIN dbo.Photos phot ON prof.photoGuid = phot.guid WHERE userID = @userID\n",
                CommandType = System.Data.CommandType.Text,
                CommandTimeout = 2000,
                Connection = this.conn
            };
            comm.Parameters.AddWithNullableValue("userID", userID);
            SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                result = new UserPhoto()
                {
                    UserID = userID,
                    ProfileID = reader.GetConverted<int>("id"),
                };
                if(reader.GetConverted<byte[]>("photo") != null)
                {
                    result.PhotoBase64 = Convert.ToBase64String(reader.GetConverted<byte[]>("photo"));
                }
            }
            reader.Close();
            return result;
        }
    }
}
