using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using PhotoSharingApp.Models;
using PhotoSharingApp.Models.Contexts;

namespace PhotoSharingApp.Controllers
{
    public class DataServiceController : ApiController
    {
        /// <summary>
        /// The data context object
        /// </summary>
        private readonly DataServiceContext _db = new DataServiceContext();

        /// <summary>
        /// Determines whether email is valid.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns>HttpResponseMessage.</returns>
        [AcceptVerbs("GET")]
        [ActionName("IsValid")]
        [HttpPost]
        public HttpResponseMessage IsValid(string property, string value)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            switch (property.ToLower())
            {
                case "username":
                    response.Content = _db.IsValidUserName(value) ? new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("0"))) : new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("1")));
                    break;
                case "email":
                    response.Content = _db.IsValidEmail(value) ? new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("0"))) : new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("1")));
                    break;
            }
            return response;
        }
    }
}
