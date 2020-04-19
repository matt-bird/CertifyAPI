using CertifyWPF.WPF_Library;
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

            string hashedPassword;
            if (String.IsNullOrEmpty(user.passwordSalt)) hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(context.Password, "SHA256");
            else hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(user.passwordSalt + context.Password, "SHA256");

            if (user!= null && 
                user.password == hashedPassword && 
                user.active == true)
            {
                // create identity
                var id = new ClaimsIdentity(context.Options.AuthenticationType);
                id.AddClaim(new Claim("sub", context.UserName));
                if (user.isUserRole("userWeb")) id.AddClaim(new Claim("role", "web"));
                if (user.isUserRole("userAuditor")) id.AddClaim(new Claim("role", "auditor"));
                if (user.isAdmin) id.AddClaim(new Claim("role", "admin"));
                context.Validated(id);
                Log.write("Access Granted for: " + user.fullName);
                return;
            }

            Log.write("Access Denied for: " + user.fullName);
            context.Rejected();
        }
    }
}