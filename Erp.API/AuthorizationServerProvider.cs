using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
//using Erp.Domain.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;
using WebMatrix.WebData;

namespace Erp.API
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            //if (!WebSecurity.Initialized)
            //{
            //    WebSecurity.InitializeDatabaseConnection("ErpDbContext", "System_User", "Id", "UserName", autoCreateTables: true);
            //}

            //if(!WebSecurity.Login(context.UserName, context.Password, persistCookie: false))
            //{
            //    context.SetError("invalid_grant", "The user name or password is incorrect.");
            //    return;
            //}

            //using (AuthRepository _repo = new AuthRepository())
            //{
            //    IdentityUser user = await _repo.FindUser(context.UserName, context.Password);

            //    if (user == null)
            //    {
            //        context.SetError("invalid_grant", "The user name or password is incorrect.");
            //        return;
            //    }
            //}

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            context.Validated(identity);
        }
    }
}