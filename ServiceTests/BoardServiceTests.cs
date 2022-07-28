using NUnit.Framework;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;
using System;

namespace ServiceTests
{
    public class BoardServiceTests
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
        public void AddBoardTest()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            string json = serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.AreEqual(new Response(), result);
        }

        [Test]
        public void AddBoardTestError()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            string json = serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [Test]
        public void RemoveBoardTest()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            string json = serviceFactory.BoardService.RemoveBoard("something@gmail.com", "board1");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.AreEqual(new Response(), result);
        }

        [Test]
        public void RemoveBoardTestError1()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            string json = serviceFactory.BoardService.RemoveBoard("something@gmail.com", "board1");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [Test]
        public void RemoveBoardTestError2()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Register("something2@gmail.com", "abC123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            serviceFactory.BoardService.JoinBoard("something2@gmail.com", 0);
            string json = serviceFactory.BoardService.RemoveBoard("something2@gmail.com", "board1");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [Test]
        public void LimitColumn()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            string json = serviceFactory.BoardService.LimitColumn("something@gmail.com", "board1", "backlog", 5);
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.AreEqual(new Response(), result);
        }

        [Test]
        public void LimitColumnError()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            string json = serviceFactory.BoardService.LimitColumn("something@gmail.com", "board1", "backlog", -10);
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [Test]
        public void GetColumnLimit()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            string json = serviceFactory.BoardService.GetColumnLimit("something@gmail.com", "board1", "backlog");
            var result = JsonSerializer.Deserialize<Response>(json);
            //int returnValue = JsonSerializer.Deserialize<int>((JsonElement)result.ReturnValue);
            Assert.AreEqual(-1, JsonSerializer.Deserialize<int>((JsonElement)result.ReturnValue));
        }

        [Test]
        public void GetColumnLimitError()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            string json = serviceFactory.BoardService.GetColumnLimit("something@gmail.com", "board1", "backlog");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [Test]
        public void AddTaskTest()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            string json = serviceFactory.BoardService.AddTask("something@gmail.com", "board1", "title1", "description1", new DateTime(2022, 12, 2));
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.AreEqual(new Response(), result);
        }

        [Test]
        public void AddTaskTestError()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            string json = serviceFactory.BoardService.AddTask("something@gmail.com", "board2", "title1", "description1", new DateTime(2022, 12, 2));
            Response? result = JsonSerializer.Deserialize<Response>(json);
            TestContext.Out.WriteLine("Message to write to log");
            Assert.IsNotNull(result.ErrorMessage);
        }

        [Test]
        public void MoveTaskTest()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            serviceFactory.BoardService.AddTask("something@gmail.com", "board1", "title1", "description1", new DateTime(2022, 12, 2));
            serviceFactory.TaskService.AssignTask("something@gmail.com", "board1", "backlog", 0, "something@gmail.com");
            string json = serviceFactory.BoardService.MoveTask("something@gmail.com", "board1", "backlog", 0);
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.AreEqual(new Response(), result);
        }

        [Test]
        public void MoveTaskTestError()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            serviceFactory.BoardService.AddTask("something@gmail.com", "board1", "title1", "description1", new DateTime(2022, 12, 2));
            string json = serviceFactory.BoardService.MoveTask("something@gmail.com", "board1", "backlog", 0);
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [Test]
        public void GetTasksInProgressTest()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            serviceFactory.BoardService.AddTask("something@gmail.com", "board1", "title1", "description1", new DateTime(2022, 12, 2));
            serviceFactory.TaskService.AssignTask("something@gmail.com", "board1", "backlog", 0, "something@gmail.com");
            serviceFactory.BoardService.MoveTask("something@gmail.com", "board1", "backlog", 0);
            string json = serviceFactory.BoardService.GetTasksInProgress("something@gmail.com");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetColumnTest()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            serviceFactory.BoardService.AddTask("something@gmail.com", "board1", "title1", "description1", new DateTime(2022, 12, 2));
            serviceFactory.BoardService.AddTask("something@gmail.com", "board1", "title2", "description1", new DateTime(2022, 12, 2));
            serviceFactory.BoardService.AddTask("something@gmail.com", "board1", "title3", "description1", new DateTime(2022, 12, 2));
            string json = serviceFactory.BoardService.GetColumnTasks("something@gmail.com","board1","backlog");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetUserBoardsTest()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Register("yehudaDragon@Dragons.fire", "abC123");

            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            serviceFactory.BoardService.AddBoard("yehudaDragon@Dragons.fire", "board2");
            serviceFactory.BoardService.AddBoard("yehudaDragon@Dragons.fire", "board3");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board4");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board5");

            serviceFactory.BoardService.JoinBoard("something@gmail.com", 1);
            serviceFactory.BoardService.TransferOwnership("yehudaDragon@Dragons.fire", "something@gmail.com", "board2");

            string json = serviceFactory.BoardService.GetUserBoards("something@gmail.com");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result);
        }

        [Test]
        public void JoinBoardTest()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            serviceFactory.UserService.Register("something2@gmail.com", "abC123");
            serviceFactory.UserService.Login("something2@gmail.com", "abC123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            string json = serviceFactory.BoardService.JoinBoard("something2@gmail.com", 0);
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.AreEqual(new Response(), result);
        }
        [Test]
        public void JoinBoardBadCaseTest()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            string json = serviceFactory.BoardService.JoinBoard("something2@gmail.com", 0);
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [Test]
        public void LeaveBoardTest()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            serviceFactory.UserService.Register("something2@gmail.com", "abC123");
            serviceFactory.UserService.Login("something2@gmail.com", "abC123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            serviceFactory.BoardService.JoinBoard("something2@gmail.com", 0);
            string json = serviceFactory.BoardService.LeaveBoard("something2@gmail.com", 0);
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.AreEqual(new Response(), result);
        }

        [Test]
        public void LeaveBoardBadCaseTest()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            serviceFactory.UserService.Register("something2@gmail.com", "abC123");
            serviceFactory.UserService.Login("something2@gmail.com", "abC123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            serviceFactory.BoardService.JoinBoard("something2@gmail.com", 0);
            string json = serviceFactory.BoardService.LeaveBoard("something@gmail.com", 0);
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [Test]
        public void TransferOwnershipTest()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            serviceFactory.UserService.Register("something2@gmail.com", "abC123");
            serviceFactory.UserService.Login("something2@gmail.com", "abC123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            serviceFactory.BoardService.JoinBoard("something2@gmail.com", 0);
            string json = serviceFactory.BoardService.TransferOwnership("something@gmail.com", "something2@gmail.com", "board1"); ;
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.AreEqual(new Response(), result);
        }

        [Test]
        public void TransferOwnershipBadCaseTest()
        {
            serviceFactory.UserService.Register("something@gmail.com", "abC123");
            serviceFactory.UserService.Login("something@gmail.com", "abC123");
            serviceFactory.UserService.Register("something2@gmail.com", "abC123");
            serviceFactory.UserService.Login("something2@gmail.com", "abC123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            string json = serviceFactory.BoardService.TransferOwnership("something@gmail.com", "something2@gmail.com", "board1"); ;
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.AreNotEqual(new Response(), result);
        }
    }
}