using NUnit.Framework;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;
using System;

namespace ServiceTests
{
    public class UserServiceTests
    {
        private ServiceFactory serviceFactory;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            serviceFactory = new ServiceFactory();
        }

        [SetUp]
        public void Setup()
        {
            serviceFactory.BoardService.DeleteAllData();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            serviceFactory.BoardService.DeleteAllData();
        }

        [Test]
        public void RegisterTest()
        {
            string json = serviceFactory.UserService.Register("something@gmail.com", "abC123");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.AreEqual(new Response(), result);
        }

        [Test]
        public void RegisterTestError1()
        {
            string json = serviceFactory.UserService.Register("something@gmail.com", "abc123");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [Test]
        public void RegisterTestError2()
        {
            string json = serviceFactory.UserService.Register("something@gmail.com", "abC5");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [Test]
        public void RegisterTestError3()
        {
            string json = serviceFactory.UserService.Register("something@gmail.com", "ABzufh");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result.ErrorMessage);
        }



        [Test]
        public void LoginTest()
        {
            string email = "something@gmail.com";
            serviceFactory.UserService.Register(email, "abC123");
            string json = serviceFactory.UserService.Login(email, "abC123");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.AreEqual(JsonSerializer.Deserialize<Response>(JsonSerializer.Serialize(new Response(null, email))).ToString(), result.ToString());
        }



        [Test]
        public void LogoutTest()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            string json = serviceFactory.UserService.Logout("something@gmail.com");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.AreEqual(new Response(), result);
        }


        [Test]
        public void LogoutTestError()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            string json = serviceFactory.UserService.Logout("something@gmail.com");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result);
        }
    }
}
