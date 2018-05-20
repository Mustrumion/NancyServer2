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
            using (SHA512 shaM = new SHA512Managed())
            {
                model.Password = Encoding.UTF8.GetString(shaM.ComputeHash(Encoding.UTF8.GetBytes(model.Password)));
            }
            Guid? guid = this.dao.LogIn(model);
            if(guid != null)
            {
                var response = new Response();
                response.StatusCode = HttpStatusCode.OK;
                response.Headers.Add("Authorization", guid.Value.ToString());
                return response;
            }
            return HttpStatusCode.Unauthorized;
        }

        private dynamic PostUser(dynamic arg)
        {
            var model = this.Bind<User>();
            using (SHA512 shaM = new SHA512Managed())
            {
                model.Password = Encoding.UTF8.GetString(shaM.ComputeHash(Encoding.UTF8.GetBytes(model.Password)));
            }
            if (this.dao.Register(model))
            {
                return HttpStatusCode.OK;
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
