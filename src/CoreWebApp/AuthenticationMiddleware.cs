using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Extensions.Logging;

namespace CoreWebApp
{
    public class AuthenticationOptions
    {
        private string _authenticationScheme;

        /// <summary>
        /// The AuthenticationScheme in the options corresponds to the logical name for a particular authentication scheme. A different
        /// value may be assigned in order to use the same authentication middleware type more than once in a pipeline.
        /// </summary>
        public string AuthenticationScheme
        {
            get { return _authenticationScheme; }
            set
            {
                _authenticationScheme = value;
                Description.AuthenticationScheme = value;
            }
        }

        /// <summary>
        /// If true the authentication middleware alter the request user coming in. If false the authentication middleware will only provide
        /// identity when explicitly indicated by the AuthenticationScheme.
        /// </summary>
        public bool AutomaticAuthenticate { get; set; }

        /// <summary>
        /// If true the authentication middleware should handle automatic challenge.
        /// If false the authentication middleware will only alter responses when explicitly indicated by the AuthenticationScheme.
        /// </summary>
        public bool AutomaticChallenge { get; set; }

        /// <summary>
        /// Gets or sets the issuer that should be used for any claims that are created
        /// </summary>
        public string ClaimsIssuer { get; set; }

        /// <summary>
        /// Additional information about the authentication type which is made available to the application.
        /// </summary>
        public AuthenticationDescription Description { get; set; } = new AuthenticationDescription();
        
    }


    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AuthenticationOptions _options;

        public AuthenticationMiddleware(
            RequestDelegate next,
            IOptions<AuthenticationOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public ILogger Logger { get; set; }

        public UrlEncoder UrlEncoder { get; set; }

        public async Task Invoke(HttpContext context)
        {
            this.BeginInvoke(context);
            await this._next.Invoke(context);
        }


        private void BeginInvoke(HttpContext context)
        {

            //The OPTIONS request does not reach this far...

            //This is to mitigate bug in Chrome - http://stackoverflow.com/questions/15734031/why-does-the-preflight-options-request-of-an-authenticated-cors-request-work-in
            //if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Credentials"))
            //{
            //    context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            //}
            //if (context.Request.Method != "OPTIONS" && !context.User.Identity.IsAuthenticated)
            //{
            //    context.Response.StatusCode = 401;
            //}

        }
    }



    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
