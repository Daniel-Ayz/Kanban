using System;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DAL;
using NUnit.Framework;

namespace BusinessTests
{
    public class BoardTests
    {
        Board board;
        string email;

        private static readonly object[] testCaseInput =
        {
            new object[] { "task1", new DateTime(2022, 12, 31), "1", "backlog" },
            new object[] { "task2", new DateTime(2022, 12, 31), "2", "backlog" },
            new object[] { "task3", new DateTime(2022, 12, 31), "3", "backlog" },
            new object[] { "task4", new DateTime(2022, 12, 31), "4", "backlog" },
            new object[] { "task5", new DateTime(2022, 12, 31), "5", "backlog" }
        };

        private static readonly object[] testBadCaseInput =
        {
            new object[] { "task1", new DateTime(2021, 12, 31), "", "backlog" },
            new object[] { null, new DateTime(2022, 12, 31), "", "backlog" },
            new object[] { "task3", new DateTime(2022, 12, 31), "", "back" },
            new object[] { "task3", new DateTime(2022, 12, 31), null, "backlog" }
        };

        [SetUp]
        public void Setup()
        {
            new BoardMapper().DeleteAllData();
            new UserMapper().DeleteAllData();
            new UserMapper().Insert(new UserDTO("Yehuda@dragonMail.com", "Dragon123"));
            board=Board.BuildStandardBoard("board1",0,"Yehuda@dragonMail.com");
            email = "Yehuda@dragonMail.com";
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            new BoardMapper().DeleteAllData();
            new UserMapper().DeleteAllData();
        }


        [TestCase("backlog")]
        [TestCase("in progress")]
        [TestCase("done")]
        [Test]
        public void GetColumnTest(string columnName)
        {
            Assert.AreEqual(columnName, board.GetColumn(columnName).name);
        }

        [Test, TestCaseSource("testCaseInput")]
        public void AddTaskTest(string title, DateTime dueDate, string description,string columnName)
        {
            board.AddTask(title,dueDate,description,columnName);
            Assert.IsTrue(board.GetColumn(columnName).ContainsTask(0));
        }

        [Test, TestCaseSource("testBadCaseInput")]
        public void AddTaskTestFail(string title, DateTime dueDate, string description, string columnName)
        {
            Assert.Catch<Exception>(() => board.AddTask(title, dueDate, description, columnName));
        }

        [TestCase("backlog", "in progress")]
        [TestCase("in progress","done")]
        [Test]
        public void MoveTaskTest(string from, string to)
        {
            board.AddTask("task1", new DateTime(2022, 12, 31), "desc1", from);
            board.GetColumn(from).GetTask(0).AssignTask("Yehuda@dragonMail.com", "Yehuda@dragonMail.com");
            board.MoveTask(email,from, to, 0);
            Assert.IsTrue(board.GetColumn(to).ContainsTask(0));
        }
        
        [Test]
        public void GetColumnTasks()
        {
            board.AddTask("task1", new DateTime(2022, 12, 31), "desc1", "backlog");
            board.AddTask("task2", new DateTime(2022, 12, 31), "desc2", "backlog");
            board.AddTask("task3", new DateTime(2022, 12, 31), "desc3", "backlog");
            board.AddTask("task4", new DateTime(2022, 12, 31), "desc4", "backlog");
            Assert.AreEqual(4, board.GetColumnTasks("backlog").Count);
        }

        [Test]
        public void BoardFunctionalityTest()
        {
            board.AddTask("task1", new DateTime(2022, 12, 31), "desc1", "backlog");
            board.AddTask("task2", new DateTime(2022, 12, 31), "desc2", "backlog");
            board.AddTask("task3", new DateTime(2022, 12, 31), "desc3", "backlog");
            board.AddTask("task4", new DateTime(2022, 12, 31), "desc4", "backlog");
            board.GetColumn("backlog").GetTask(0).AssignTask(email, email);
            board.MoveTask(email,"backlog", "in progress", 0);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(3, board.GetColumnTasks("backlog").Count);
                Assert.AreEqual(1, board.GetColumnTasks("in progress").Count);
                Assert.AreEqual(0, board.GetColumnTasks("done").Count);
            });
        }

    }
}