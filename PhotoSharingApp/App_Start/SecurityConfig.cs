﻿using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoSharingApp.App_Start
{
    public class BasicAuthenticationHandler : DelegatingHandler
    {
        private const string WwwAuthenticateHeader = "WWW-Authenticate";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            var credentials = ParseAuthorizationHeader(request);

            if (credentials != null)
            {
                var identity = new BasicAuthenticationIdentity(credentials.Name, credentials.Password);
                var principal = new GenericPrincipal(identity, null);
                Thread.CurrentPrincipal = principal;
                //if (HttpContext.Current != null)
                //    HttpContext.Current.User = principal;
            }

            return base.SendAsync(request, cancellationToken)
                .ContinueWith(task =>
                {
                    var response = task.Result;
                    if (credentials == null && response.StatusCode == HttpStatusCode.Unauthorized)
                        Challenge(request, response);


                    return response;
                });
        }

        /// <summary>
        /// Parses the Authorization header and creates user credentials
        /// </summary>
        /// <param name="request"></param>
        protected virtual BasicAuthenticationIdentity ParseAuthorizationHeader(HttpRequestMessage request)
            {
            if (request == null) throw new ArgumentNullException("request");
            string authHeader = null;
            var auth = request.Headers.Authorization;
            if (auth != null && auth.Scheme == "Basic")
                authHeader = auth.Parameter;

            if (string.IsNullOrEmpty(authHeader))
                return null;

            authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));

            var tokens = authHeader.Split(':');
            if (tokens.Length < 2)
                return null;

            return new BasicAuthenticationIdentity(tokens[0], tokens[1]);
        }


        /// <summary>
        /// Send the Authentication Challenge request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        static void Challenge(HttpRequestMessage request, HttpResponseMessage response)
        {
            var host = request.RequestUri.DnsSafeHost;
            response.Headers.Add(WwwAuthenticateHeader, string.Format("Basic realm=\"{0}\"", host));
        }
    }
    public class BasicAuthenticationIdentity : GenericIdentity
    {
        public BasicAuthenticationIdentity(string name, string password)
            : base(name, "Basic")
        {
            Password = password;
        }

        /// <summary>
        /// Basic Auth Password for custom authentication
        /// </summary>
        public string Password { get; set; }
    }
}