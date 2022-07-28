using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DAL;
using NUnit.Framework;

namespace BusinessTests
{
    public class BoardControllerTests
    {
        UserController userController;
        BoardController boardController;

        [SetUp]
        public void Setup()
        {

            new UserMapper().DeleteAllData();
            new BoardMapper().DeleteAllData();
            userController = new UserController();
            boardController = new BoardController();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            new BoardMapper().DeleteAllData();
            new UserMapper().DeleteAllData();
        }

        [Test]
        public void clear()
        {
            
        }

        [Test]
        public void AddBoardTest()
        {
            string email = "something@gmail.com";
            string password = "abC123";
            userController.Register(email, password);
            User user = userController.GetUser(email);
            user.Login(password);
            boardController.AddBoard(email, "board1");
            Assert.IsTrue(boardController.IsMemberOfBoard(email, "board1"));
        }

        [Test]
        public void AddBoardTestError1()
        {
            string email = "something@gmail.com";
            string password = "abC123";
            userController.Register(email, password);
            User user = userController.GetUser(email);
            user.Login(password);
            Assert.Catch<Exception>(() => boardController.AddBoard(email, null));
        }

        [Test]
        public void AddBoardTestError2()
        {
            string email = "something@gmail.com";
            string password = "abC123";
            userController.Register(email, password);
            User user = userController.GetUser(email);
            user.Login(password);
            boardController.AddBoard(email, "board1");
            Assert.Catch<Exception>(() => boardController.AddBoard(email, "board1"));
        }

        [Test]
        public void RemoveBoardTest()
        {
            string email = "something@gmail.com";
            string password = "abC123";
            userController.Register(email, password);
            User user = userController.GetUser(email);
            user.Login(password);
            boardController.AddBoard(email, "board1");
            boardController.RemoveBoard(email, "board1");
            Assert.IsFalse(boardController.IsMemberOfBoard(email, "board1"));
        }

        [Test]
        public void RemoveBoardTestError1()
        {
            string email = "something@gmail.com";
            string password = "abC123";
            userController.Register(email, password);
            User user = userController.GetUser(email);
            user.Login(password);
            Assert.Catch<Exception>(() => boardController.RemoveBoard(email, null));
        }

        [Test]
        public void RemoveBoardTestError2()
        {
            string email = "something@gmail.com";
            string password = "abC123";
            userController.Register(email, password);
            User user = userController.GetUser(email);
            user.Login(password);
            Assert.Catch<Exception>(() => boardController.RemoveBoard(email, "board1"));
        }

        [Test]
        public void GetBoardTest()
        {
            string email = "something@gmail.com";
            string password = "abC123";
            userController.Register(email, password);
            User user = userController.GetUser(email);
            user.Login(password);
            boardController.AddBoard(email, "board1");
            Board board = boardController.GetBoard(email, "board1");
            Assert.AreEqual("board1", board.name);
        }

        [Test]
        public void GetBoardTestError()
        {
            string email = "something@gmail.com";
            string password = "abC123";
            userController.Register(email, password);
            User user = userController.GetUser(email);
            user.Login(password);
            Assert.Catch<Exception>(() => boardController.GetBoard(email, "board1"));
        }

        [Test]
        public void GetTasksInProgressTest()
        {
            string email = "something@gmail.com";
            string password = "abC123";
            userController.Register(email, password);
            User user = userController.GetUser(email);
            user.Login(password);
            boardController.AddBoard(email, "board1");
            Board board1 = boardController.GetBoard(email, "board1");
            board1.AddTask("task1", new DateTime(2022, 12, 31), "important", "in progress");
            board1.GetColumn("in progress").GetTask(0).AssignTask(email, email);
            List<Task> tasks = boardController.GetTasksInProgress(email);
            string title = tasks[0].Title;
            Assert.AreEqual("task1", title);
        }

        [Test]
        public void GetUserBoardsTest()
        {
            string email = "something@gmail.com";
            string password = "abC123";
            userController.Register(email, password);
            List<int> boards = boardController.GetUserBoards(email);
            Assert.AreEqual(new List<int>(), boards);
        }

        [Test]
        public void JoinBoardTest()
        {
            string email1 = "something1@gmail.com";
            string email2 = "something2@gmail.com";
            string password = "abC123";
            userController.Register(email1, password);
            userController.Register(email2, password);
            
            boardController.AddBoard(email1, "board1");
            boardController.JoinBoard(email2, 0);
            Board board = boardController.GetBoard(email2, "board1");
            Assert.AreEqual(0, board.id);
        }

        [Test]
        public void JoinBoardTestError()
        {
            string email1 = "something1@gmail.com";
            string password = "abC123";
            userController.Register(email1, password);
            Assert.Catch<Exception>(() => boardController.JoinBoard(email1, 0));
        }

        [Test]
        public void LeaveBoardTest()
        {
            string email1 = "something1@gmail.com";
            string email2 = "something2@gmail.com";
            string password = "abC123";
            userController.Register(email1, password);
            userController.Register(email2, password);

            boardController.AddBoard(email1, "board1");
            boardController.JoinBoard(email2, 0);
            boardController.LeaveBoard(email2, 0);
            Assert.IsFalse(boardController.IsMemberOfBoard(email2, "board1"));
        }

        [Test]
        public void LeaveBoardTestError()
        {
            string email1 = "something1@gmail.com";
            string password = "abC123";
            userController.Register(email1, password);

            boardController.AddBoard(email1, "board1");
            Assert.Catch<Exception>(() => boardController.LeaveBoard(email1, 0));
        }

        [Test]
        public void LoadDataTest()
        {
            string email = "something@gmail.com";
            string password = "abC123";
            userController.Register(email, password);
        }
    }
}
