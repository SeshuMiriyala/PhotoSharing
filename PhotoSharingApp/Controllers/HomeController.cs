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
    public class KeyValueObject
    {
        public string Key { get; set; }
    }
    public class RegisterParams
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
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
                    response.Content = new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("0")));
                }
                else
                {
                    response = request.CreateResponse(HttpStatusCode.OK);
                    if(String.IsNullOrEmpty(loginModel.Password))
                        response.Content = new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("1")));
                    else
                        response.Content = new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("2")));
                }
            }
            else
            {
                response = request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("0")));
            }
            return response;
        }

        [AcceptVerbs("POST")]
        [ActionName("Register")]
        [HttpPost]
        public HttpResponseMessage Register(HttpRequestMessage request)
        {
            var stream = request.Content.ReadAsStringAsync().Result;
            var registerModel = Newtonsoft.Json.JsonConvert.DeserializeObject<RegisterParams>(Encoding.Default.GetString(Convert.FromBase64String(stream)));
            var response = request.CreateResponse(HttpStatusCode.OK);
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

        [AcceptVerbs("POST")]
        [ActionName("IsValidEmail")]
        [HttpPost]
        public HttpResponseMessage IsValidEmail(HttpRequestMessage request)
        {
            var stream = request.Content.ReadAsStringAsync().Result;
            var registerModel = Newtonsoft.Json.JsonConvert.DeserializeObject < KeyValueObject>(Encoding.Default.GetString(Convert.FromBase64String(stream)));
            var response = request.CreateResponse(HttpStatusCode.OK);
            if (_db.IsValidEmail(registerModel.Key))
                response.Content = new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("0")));
            else
                response.Content = new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("1")));
            return response;
        }

        [AcceptVerbs("POST")]
        [ActionName("IsValidUserName")]
        [HttpPost]
        public HttpResponseMessage IsValidUserName(HttpRequestMessage request)
        {
            var stream = request.Content.ReadAsStringAsync().Result;
            var registerModel = Newtonsoft.Json.JsonConvert.DeserializeObject<KeyValueObject>(Encoding.Default.GetString(Convert.FromBase64String(stream)));
            var response = request.CreateResponse(HttpStatusCode.OK);
            if (_db.IsValidUserName(registerModel.Key))
                response.Content = new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("0")));
            else
                response.Content = new StringContent(Convert.ToBase64String(Encoding.Default.GetBytes("1")));
            return response;
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
