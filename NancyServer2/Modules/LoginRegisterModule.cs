using Nancy;
using Nancy.ModelBinding;
using NancyServer2.DAOs;
using NancyServer2.Objects;
using System;
using System.Security.Cryptography;
using System.Text;

namespace NancyServer2.Modules
{
    /// <summary>
    /// This all should be on separate WS, using OAuth or some other tried auth method. But it's good enough for our purposes. 
    /// </summary>
    public class LoginRegisterModule : NancyModule
    {
        UserDAO dao;

        /// <summary>
        /// Do debugowania, w przyszłości należy wyłączyć.
        /// </summary>
        public LoginRegisterModule()
        {
            this.dao = new UserDAO();
            Get["/users/{id}"] = GetUser;
            Post["/users"] = PostUser;
            Post["/login"] = PostLogin;
            Post["/checktoken"] = PostToken;
        }

        private dynamic PostToken(dynamic arg)
        {
            Token model = null;
            try
            {
                model = this.Bind<Token>();
            }
            catch
            {
                return Negotiate.WithModel("Incorrect object structure.").WithStatusCode(HttpStatusCode.BadRequest);
            }
            string error = dao.IsTokenValid(model);
            if (String.IsNullOrEmpty(error))
            {
                return HttpStatusCode.OK;
            }
            else
            {
                return Negotiate.WithModel(error).WithStatusCode(HttpStatusCode.Unauthorized);
            }
        }

        private dynamic PostLogin(dynamic arg)
        {
            User model = null;
            try
            {
                model = this.Bind<User>();
            }
            catch
            {
                return Negotiate.WithModel("Incorrect object structure.").WithStatusCode(HttpStatusCode.BadRequest);
            }
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return HttpStatusCode.BadRequest;
            }
            Token session = this.dao.LogIn(model);
            if(session != null)
            {
                var response = new Response();
                return Negotiate.WithModel(session).WithHeader("Authorization", session.SessionID.ToString()).WithStatusCode(HttpStatusCode.OK);
            }
            return HttpStatusCode.Unauthorized;
        }

        public dynamic PostUser(dynamic arg)
        {
            User model = null;
            try
            {
                model = this.Bind<User>();
            }
            catch
            {
                return Negotiate.WithModel("Incorrect object structure.").WithStatusCode(HttpStatusCode.BadRequest);
            }
            if (this.dao.Register(model))
            {
                User user = this.dao.GetUserByEmail(model.Email);
                return user;
            }
            else
            {
                return HttpStatusCode.BadRequest;
            }
        }

        private dynamic GetUser(dynamic arg)
        {
            User user = dao.GetUser(arg.id);
            if(user == null)
            {
                return HttpStatusCode.BadRequest;
            }
            return user;
        }
    }
}
