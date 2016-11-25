using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
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
    
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        
        public async Task Invoke(HttpContext context)
        {
            this.BeginInvoke(context);
            await this._next.Invoke(context);
        }


        private async void BeginInvoke(HttpContext context)
        {
            //This is to mitigate bug in Chrome - http://stackoverflow.com/questions/15734031/why-does-the-preflight-options-request-of-an-authenticated-cors-request-work-in
            //See also https://github.com/aspnet/CORS/issues/60
            if (context.Request.Method!="OPTIONS" && !context.User.Identity.IsAuthenticated)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Not Authenticated");
            }
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
