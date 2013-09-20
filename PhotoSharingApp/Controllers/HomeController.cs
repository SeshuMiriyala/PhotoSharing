// ***********************************************************************
// Assembly         : PhotoSharingApp
// Author           : Seshu Miriyala
// Created          : 09-12-2013
//
// Last Modified By : Seshu Miriyala
// Last Modified On : 09-18-2013
// ***********************************************************************
// <copyright file="HomeController.cs" company="Coding Bugs">
//     Copyright (c) Coding Bugs. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using PhotoSharingApp.Models;
using PhotoSharingApp.Models.Contexts;
using PhotoSharingApp.Models.Security;

namespace PhotoSharingApp.Controllers
{
    /// <summary>
    /// Controller for Home page.
    /// </summary>
    public class HomeController : ApiController
    {
        /// <summary>
        /// The data context object
        /// </summary>
        private readonly HomeContext _db = new HomeContext();

        /// <summary>
        /// Verifies the Login parameters nd generates a session Id.
        /// </summary>
        /// <param name="loginModel">Login model</param>
        /// <returns>HttpResponseMessage.</returns>
        [AcceptVerbs("POST")]
        [ActionName("Login")]
        [HttpPost]
        public HttpResponseMessage Login(LoginParams loginModel)
        {
            HttpResponseMessage response;
            if (ModelState.IsValid)
            {
                var token = AccessTokenRepository.GetToken(loginModel.UserName);
                if (token == null)
                {
                    var isValidUser = _db.IsValidUser(loginModel.UserName, loginModel.Password);
                    if (isValidUser)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK);
                        AccessTokenManager.GenerateAccessToken(loginModel.UserName);
                        response.Content = new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("0")));
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK);
                        response.Content = String.IsNullOrEmpty(loginModel.Password)
                                               ? new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("1")))
                                               : new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("2")));
                    }
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("0")));
                }
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return response;
        }

        /// <summary>
        /// Sign up a new user.
        /// </summary>
        /// <param name="registerModel">SignUpParams Model</param>
        /// <returns>HttpResponseMessage.</returns>
        [AcceptVerbs("POST")]
        [ActionName("Register")]
        [HttpPost]
        public HttpResponseMessage SignUp(SignUpParams registerModel)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            if (_db.RegisterUser(registerModel.UserName, registerModel.Password, registerModel.FirstName,
                                 registerModel.LastName, registerModel.MiddleName, registerModel.Email))
            {
                response.Content = new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("0")));
                AccessTokenManager.GenerateAccessToken(registerModel.UserName);
            }
            else
                response.Content = new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("2")));
            return response;
        }

        /// <summary>
        /// Determines whether email is valid.
        /// </summary>
        /// <param name="email">Email Object</param>
        /// <returns>HttpResponseMessage.</returns>
        [AcceptVerbs("POST")]
        [ActionName("IsValidEmail")]
        [HttpPost]
        public HttpResponseMessage IsValidEmail(KeyValueObject email)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = _db.IsValidEmail(email.Key) ? new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("0"))) : new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("1")));
            return response;
        }

        /// <summary>
        /// Determines whether user name is valid.
        /// </summary>
        /// <param name="userName">UserName object</param>
        /// <returns>HttpResponseMessage.</returns>
        [AcceptVerbs("POST")]
        [ActionName("IsValidUserName")]
        [HttpPost]
        public HttpResponseMessage IsValidUserName(KeyValueObject userName)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = _db.IsValidUserName(userName.Key) ? new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("0"))) : new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("1")));
            return response;
        }
    }
}
