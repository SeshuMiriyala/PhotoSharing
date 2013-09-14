using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using PhotoSharingApp.Models.Contexts;
using PhotoSharingApp.Models.Security;

namespace PhotoSharingApp.Controllers
{
    public class LoginParams
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class HomeController : ApiController
    {
        private HomeContext _db = new HomeContext();
        // GET api/home
        //[AcceptVerbs("POST")]
        //[ActionName("Login")]
        //[HttpPost]
        //public bool Login(string userName, string password)
        //{
        //    return _db.IsValidUser(userName, password);
        //}

        [AcceptVerbs("POST")]
        [ActionName("Login")]
        [HttpPost]
        public HttpResponseMessage Login(HttpRequestMessage request)
        {
            var stream = request.Content.ReadAsStringAsync().Result;
            var loginModel = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginParams>(Encoding.Default.GetString(Convert.FromBase64String(stream)));
            var token = AccessTokenRepository.GetToken(loginModel.UserName);
            HttpResponseMessage response;
            if (token == null)
            {
                var isValidUser = _db.IsValidUser(loginModel.UserName, loginModel.Password);
                if (isValidUser)
                {
                    response = request.CreateResponse(HttpStatusCode.OK);
                    AccessTokenManager.GenerateAccessToken(loginModel.UserName);
                }
                else
                {
                    response = request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                response = request.CreateResponse(HttpStatusCode.OK);
            }
            return response;
        }

        [AcceptVerbs("POST")]
        [ActionName("Register")]
        [HttpPost]
        public bool Register(string userName, string password, string firstName, string lastName, string middleName, string emailId)
        {
            return _db.RegisterUser(userName, password, firstName, lastName, middleName, emailId);
        }

        // GET api/home/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/home
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT api/home/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/home/5
        public void Delete(int id)
        {
        }
    }
}
