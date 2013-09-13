using System.Web;
using System.Web.Http;
using PhotoSharingApp.Models.Contexts;

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
        public bool Login(LoginParams data)
        {
            var stream = HttpContext.Current.Request.InputStream;
            return _db.IsValidUser(data.UserName, data.Password);
        }

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
        public void Post([FromBody]string value)
        {
        }

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
