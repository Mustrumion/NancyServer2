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
            Get["/user/{id}/profile"] = GetProfile;
            Post["/user/{id}/profile"] = PostProfile;
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
            if(profile == null)
            {
                User user = dao.GetUser(arg.id);
                if (user == null)
                {
                    return HttpStatusCode.BadRequest;
                }
                else
                {
                    return new UserProfile()
                    {
                        UserID = user.ID,
                        Nick = user.Email.GetUntilOrEmpty("@")
                    };
                }
            }
            return profile;
        }
    }
}
