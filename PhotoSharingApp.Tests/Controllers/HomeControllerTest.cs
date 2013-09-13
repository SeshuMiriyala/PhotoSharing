using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhotoSharingApp.Controllers;

namespace PhotoSharingApp.Tests.Controllers
{
    /// <summary>
    /// Summary description for HomeControllerTest
    /// </summary>
    [TestClass]
    public class HomeControllerTest
    {
        private HomeController _target;
        private const string ValidUserName = "sa";
        private const string ValidPassword = "sa";

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        #endregion
        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        public void MyTestInitialize()
        {
            _target = new HomeController();
        }
        
        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void MyTestCleanup()
        {
            _target.Dispose();
        }
        
        [TestMethod]
        public void TestLoginForValidCredentials()
        {
            Assert.IsTrue(_target.Login(ValidUserName, ValidPassword));
        }

        [TestMethod]
        public void TestLoginForInValidCredentials()
        {
            Assert.IsFalse(_target.Login(ValidUserName + '1', ValidPassword));
        }

        [TestMethod]
        public void TestRegisterForValidUser()
        {
            Assert.IsTrue(_target.Register(ValidUserName,ValidPassword, ValidUserName, ValidUserName, ValidUserName, ValidUserName + "@gmail.com"));
        }

        [TestMethod]
        //[ExpectedException(typeof(SqlException))]
        public void TestRegisterForInValidUser()
        {
            Assert.IsFalse(_target.Register(ValidUserName, ValidPassword, ValidUserName, ValidUserName, ValidUserName, ValidUserName + "@gmail.com"));
        }
    }
}
