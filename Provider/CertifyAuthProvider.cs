using CertifyWPF.WPF_User;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace CertifyWPF.Provider
{
    public class CertifyAuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // OAuth2 supports the notion of client authentication
            // this is not used here
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // Password Success
            User user = User.getFromEmailAddress(context.UserName);

            if (user!= null && 
                user.password == context.Password && 
                user.active == true)
            {
                // create identity
                var id = new ClaimsIdentity(context.Options.AuthenticationType);
                id.AddClaim(new Claim("sub", context.UserName));
                id.AddClaim(new Claim("role", "user"));

                context.Validated(id);
                return;
            }

            else  context.Rejected();
        }
    }
}