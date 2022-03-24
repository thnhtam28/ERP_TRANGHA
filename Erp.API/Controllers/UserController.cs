using Erp.API.Models;
using Erp.Domain.Account.Repositories;
using Erp.Domain.Entities;
using Erp.Domain.Helper;
using Erp.Domain.Interfaces;
using Erp.Domain.Repositories;
using Erp.Domain.Sale.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebMatrix.WebData;

namespace Erp.API.Controllers
{
    public class UserController : ApiController
    {
        [HttpPost]
        [System.Web.Mvc.ValidateAntiForgeryToken]
        public HttpResponseMessage Login([FromBody] UserModel model)
        {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                model.UserName = model.UserName.ToLower();
                UserRepository userRepository = new UserRepository(new Domain.ErpDbContext());

                User user = null;

                user = userRepository.GetByUserName(model.UserName);
                //cập nhật id app mobile để send notification
                if ((model.PlayerId != null) && (model.PlayerId != ""))
                {
                    user.PlayerId = model.PlayerId;
                    userRepository.UpdateUser(user);
                }
                if (user != null && user.LoginFailedCount >= 5)
                {
                    resp.Content = new StringContent(JsonConvert.SerializeObject("Bạn đăng nhập sai quá 5 lần. Tài khoản này đã bị khóa. Vui lòng liên hệ quản trị để được hỗ trợ!"));
                    return resp;
                }

                if (!WebSecurity.Initialized)
                {
                    WebSecurity.InitializeDatabaseConnection("ErpDbContext", "System_User", "Id", "UserName", autoCreateTables: true);
                }

                if (WebSecurity.Login(model.UserName, model.Password))
                {
                    var vwUser = userRepository.GetByvwUserName(model.UserName);

                    if (vwUser != null)
                    {
                        userRepository.resetLoginFailed(vwUser.Id);

                        resp.Content = new StringContent(JsonConvert.SerializeObject(vwUser));
                    }
                    else
                    {
                        resp.Content = new StringContent(JsonConvert.SerializeObject("fail"));
                    }
                }
                else
                {
                    resp.Content = new StringContent(JsonConvert.SerializeObject("fail"));
                }
            }
            catch (Exception ex)
            {
                resp.Content = new StringContent(JsonConvert.SerializeObject(ex.Message));
            }

            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }




        [HttpPost]
        public HttpResponseMessage ChangePassword([FromBody] UserModel model)
        {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                UserRepository userRepository = new UserRepository(new Domain.ErpDbContext());
                string pUserName = model.UserName;
                string pPassWord_newpass = model.ConfirmPassword;
                string pPassWord_current = model.Password;


                if (!WebSecurity.Initialized)
                {
                    WebSecurity.InitializeDatabaseConnection("ErpDbContext", "System_User", "Id", "UserName", autoCreateTables: true);
                }

                if (WebSecurity.ChangePassword(pUserName, pPassWord_current, pPassWord_newpass) == true)
                {
                    var vwUser = userRepository.GetByvwUserName(model.UserName);

                    if (vwUser != null)
                    {
                        userRepository.resetLoginFailed(vwUser.Id);

                        resp.Content = new StringContent(JsonConvert.SerializeObject(vwUser));
                    }
                    else
                    {
                        resp.Content = new StringContent(JsonConvert.SerializeObject("fail"));
                    }
                }
                else
                {
                    resp.Content = new StringContent(JsonConvert.SerializeObject("fail"));
                }

            }
            catch (Exception ex)
            {
                resp.Content = new StringContent(JsonConvert.SerializeObject(ex.Message));
            }
            return resp;

        }





    }
}