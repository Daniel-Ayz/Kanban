using System;
using IntroSE.Kanban.Backend.BusinessLayer;
using NUnit.Framework;
using IntroSE.Kanban.Backend.DAL;

namespace BusinessTests
{
    public class UserTests
    {
        User user;

        [SetUp]
        public void Setup()
        {
            new UserMapper().DeleteAllData();
            user = User.BuildUser("email1@gmail.com", "abcde123");
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            new BoardMapper().DeleteAllData();
            new UserMapper().DeleteAllData();
        }

        [Test]
        public void enteredtest()
        {
            User user1 = User.BuildUser("email2@gmail.com", "abcde1234");
            User user2 = User.BuildUser("email3@gmail.com", "Abcde1234");
            User user3 = User.BuildUser("email4@gmail.com", "Bbcde1234");
        }

        [Test]
        public void enteredtest_badCase()
        {
            User user1 = User.BuildUser("email2@gmail.com", "abcde1234");
            User user2 = User.BuildUser("email2@gmail.com", "abcde1234");
        }

        [Test]
        public void loginTest()
        {
            user.Login("abcde123");
            Assert.IsTrue(user.LoggedIn);
        }

        [Test]
        public void loginTest_badCase()
        {
            user.Logout();
            //trying to login with wrong password
            var ex = Assert.Throws<Exception>(() => user.Login("abcde12"));
            Assert.AreEqual("Error: The password is incorrect", ex.Message);
            
        }

        [Test]
        public void logoutTest()
        {
            user.Logout();
            Assert.IsTrue(!user.LoggedIn);
        }

        [Test]
        public void logoutTest_badCase()
        {
            user.Logout();
            //trying to logout without be logged in 
            var ex = Assert.Throws<Exception>(() => user.Logout());
            Assert.AreEqual("Error: The user is not logged in", ex.Message);
        }

    }
}