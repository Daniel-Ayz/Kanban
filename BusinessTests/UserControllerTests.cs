using System;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DAL;
using NUnit.Framework;

namespace BusinessTests
{
    public class UserControllerTests
    {
        UserController userController;

        [SetUp]
        public void Setup()
        {
            userController = new UserController();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            new BoardMapper().DeleteAllData();
            new UserMapper().DeleteAllData();
        }

        [Test]
        public void RegisterTest()
        {
            string email = "something@gmail.com";
            string password = "abC123";
            userController.Register(email, password);
            Assert.True(userController.UserExists(email));
        }

        [Test]
        public void RegisterTestError1()
        {
            string email = "something@gmail.com";
            string password = "abC12";
            Assert.Catch<Exception>(() => userController.Register(email,password));
        }

        [Test]
        public void RegisterTestError2()
        {
            string email = "something@gmail.com";
            string password = "abC123";
            userController.Register(email, password);  
            Assert.Catch<Exception>(() => userController.Register(email, password));
        }

        [Test]
        public void RegisterTestError3()
        {
            string email = "somethinggmail.com";
            string password = "abC123";
            Assert.Catch<Exception>(() => userController.Register(email, password));
        }

        [Test]
        public void RegisterTestError4()
        {
            string email = null;
            string password = "abC123";
            Assert.Catch<Exception>(() => userController.Register(email, password));
        }

        [Test]
        public void DeleteUserTest()
        {
            string email = "something@gmail.com";
            string password = "abC123";
            userController.Register(email, password);
            userController.DeleteUser(email);
            Assert.IsFalse(userController.UserExists(email));
        }

        [Test]
        public void DeleteUserTestError()
        {
            string email = "something@gmail.com";
            string password = "abC123";
            Assert.Catch<Exception>(() => userController.DeleteUser(email));
        }

        [Test]
        public void IsLoggedInTest()
        {
            string email = "something@gmail.com";
            string password = "abC123";
            userController.Register(email, password);
            Assert.IsTrue(userController.IsLoggedIn(email));
        }

        [Test]
        public void IsLoggedInTestError()
        {
            string email = "something@gmail.com";
            string password = "abC123";
            Assert.Catch<Exception>(() => userController.IsLoggedIn(email));
        }

        [Test]
        public void GetUserTest()
        {
            string email = "something@gmail.com";
            string password = "abC123";
            userController.Register(email, password);
            User user = userController.GetUser(email);
            Assert.AreEqual(email, user.email);
        }

        [Test]
        public void GetUserTestError()
        {
            string email = "something@gmail.com";
            Assert.Catch<Exception>(() => userController.GetUser(email));
        }
    }
}
