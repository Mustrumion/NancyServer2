using Nancy;
using Nancy.ModelBinding;
using NancyServer2.DAOs;
using NancyServer2.Objects;
using System;
using System.Security.Cryptography;
using System.Text;
using static NancyServer2.DAOs.SQLServerBaseDAO;

namespace NancyServer2.Modules
{
    /// <summary>
    /// This all should be on separate WS, using OAuth or some other tried auth method. But it's good enough for our purposes. 
    /// </summary>
    public class UserProfileModule : NancyModule
    {
        UserProfileDAO dao;

        /// <summary>
        /// Do debugowania, w przyszłości należy wyłączyć.
        /// </summary>
        public UserProfileModule()
        {
            this.dao = new UserProfileDAO();
            Get["/users/{id}/profile"] = GetProfile;
            Post["/users/{id}/profile"] = PostProfile;
            Get["/users/{id}/photo"] = GetProfilePhoto;
            Post["/users/{id}/photo"] = PostProfilePhoto;
        }


        public dynamic PostProfile(dynamic arg)
        {
            //TODO check if token is good and user is authorized
            UserProfile model = null;
            try
            {
                model = this.Bind<UserProfile>();
            }
            catch
            {
                return Negotiate.WithModel("Incorrect object structure.").WithStatusCode(HttpStatusCode.BadRequest);
            }
            dao.SaveUserProfile(model);
            return dao.GetUserProfile(model.UserID);
        }

        private dynamic GetProfile(dynamic arg)
        {
            UserProfile profile = dao.GetUserProfile(arg.id);
            if (profile == null)
            {
                User user = dao.GetUser(arg.id);
                if (user == null)
                {
                    return HttpStatusCode.BadRequest;
                }
                else
                {
                    profile = new UserProfile()
                    {
                        UserID = user.ID,
                        Nick = user.Email.GetUntilOrEmpty("@")
                    };
                    dao.SaveUserProfile(profile);
                    return profile;
                }
            }
            return profile;
        }

        public dynamic PostProfilePhoto(dynamic arg)
        {
            //TODO check if token is good and user is authorized
            UserPhoto model = null;
            try
            {
                model = this.Bind<UserPhoto>();
            }
            catch(Exception e)
            {
                return Negotiate.WithModel("Incorrect object structure.").WithStatusCode(HttpStatusCode.BadRequest);
            }
            dao.SaveUserProfilePhoto(model);
            return dao.GetUserProfilePhoto(model.UserID);
        }

        private dynamic GetProfilePhoto(dynamic arg)
        {
            UserPhoto photo = dao.GetUserProfilePhoto(arg.id);
            if (photo == null)
            {
                return HttpStatusCode.BadRequest;
            }
            return photo;
        }
    }
}
