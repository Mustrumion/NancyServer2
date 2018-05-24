using Nancy;
using Nancy.ModelBinding;
using NancyServer2.DAOs;
using NancyServer2.Objects;
using System;
using System.Security.Cryptography;
using System.Text;

namespace NancyServer2.Modules
{
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
        }

        private dynamic PostLogin(dynamic arg)
        {
            var model = this.Bind<User>();
            if(string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
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

        private dynamic PostUser(dynamic arg)
        {
            var model = this.Bind<User>();
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
