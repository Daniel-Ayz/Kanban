using NUnit.Framework;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;
using System;

namespace ServiceTests
{
    internal class TaskServiceTests
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
        public void UpdateTaskDueDateTest()
        {
            serviceFactory.UserService.Register("something@gmail.com", "Abcde123");
            serviceFactory.UserService.Login("something@gmail.com", "Abcde123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            serviceFactory.BoardService.AddTask("something@gmail.com", "board1", "task1", "check1", new DateTime(2022, 10, 10));
            serviceFactory.TaskService.AssignTask("something@gmail.com", "board1", "backlog", 0, "something@gmail.com");
            string json = serviceFactory.TaskService.UpdateTaskDueDate("something@gmail.com", "board1", "backlog", 0, new DateTime(2023, 10, 2));
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.AreEqual(result, new Response());
        }

        [Test]
        public void UpdateTaskDueDateTest_badCase()
        {
            // insert due date that passed
            serviceFactory.UserService.Register("something@gmail.com", "Abcde123");
            serviceFactory.UserService.Login("something@gmail.com", "Abcde123");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            serviceFactory.BoardService.AddTask("something@gmail.com", "board1", "task1", "check1", new DateTime(2022, 10, 10));
            serviceFactory.TaskService.AssignTask("something@gmail.com", "board1", "backlog", 0, "something@gmail.com");
            string json = serviceFactory.TaskService.UpdateTaskDueDate("something@gmail.com", "board1", "backlog", 0, new DateTime(2021, 10, 2));
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result.ErrorMessage);
        }
        [Test]
        public void UpdateTaskTitleTest()
        {
            serviceFactory.UserService.Register("something1@gmail.com", "ABcde123");
            serviceFactory.UserService.Login("something1@gmail.com", "ABcde123");
            serviceFactory.BoardService.AddBoard("something1@gmail.com", "board1");
            serviceFactory.BoardService.AddTask("something1@gmail.com", "board1", "task1", "check1", new DateTime(2022, 12, 12));
            serviceFactory.TaskService.AssignTask("something1@gmail.com", "board1", "backlog", 0, "something1@gmail.com");
            string json = serviceFactory.TaskService.UpdateTaskTitle("something1@gmail.com", "board1", "backlog", 0, "sometitle");

            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.AreEqual(result , new Response());
        }

        [Test]
        public void UpdateTaskTitleTest_badCase1()
        {
            // insert empty title
            serviceFactory.UserService.Register("something1@gmail.com", "ABcde123");
            serviceFactory.UserService.Login("something1@gmail.com", "ABcde123");
            serviceFactory.BoardService.AddBoard("something1@gmail.com", "board1");
            serviceFactory.BoardService.AddTask("something1@gmail.com", "board1", "task1", "check1", new DateTime(2022, 12, 12));
            serviceFactory.TaskService.AssignTask("something@gmail.com", "board1", "backlog", 0, "something@gmail.com");
            string json = serviceFactory.TaskService.UpdateTaskTitle("something1@gmail.com", "board1", "done", 0, "");

            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [Test]
        public void UpdateTaskTitleTest_badCase2()
        {
            // insert invalid title
            serviceFactory.UserService.Register("something1@gmail.com", "ABcde123");
            serviceFactory.UserService.Login("something1@gmail.com", "ABcde123");
            serviceFactory.BoardService.AddBoard("something1@gmail.com", "board1");
            serviceFactory.BoardService.AddTask("something1@gmail.com", "board1", "task1", "check1", new DateTime(2022, 12, 12));
            serviceFactory.TaskService.AssignTask("something@gmail.com", "board1", "backlog", 0, "something@gmail.com");
            string json = serviceFactory.TaskService.UpdateTaskTitle("something1@gmail.com", "board1", "backlog", 0, "ghfhakgiehcjgkasnckglapeotkshvkgkslclgrejhgh;ghre;jkgrh fjvfg;lskjg lgjdflkg;jsdr;gljs kvjdflk'gjsdf'l ");

            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [Test]
        public void UpdateTaskDescriptionTest()
        {
            serviceFactory.UserService.Register("something@gmail.com", "ABCde1");
            serviceFactory.UserService.Login("something@gmail.com", "ABCde1");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            serviceFactory.BoardService.AddTask("something@gmail.com", "board1", "task1", "check1", new DateTime(2022, 12, 1));
            serviceFactory.TaskService.AssignTask("something@gmail.com", "board1", "backlog", 0, "something@gmail.com");

            string json = serviceFactory.TaskService.UpdateTaskDescription("something@gmail.com", "board1", "backlog", 0, "somedescription");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.AreEqual(result, new Response());
        }

        [Test]
        public void UpdateTaskDescriptionTest_badCase1()
        {
            // insert null description
            serviceFactory.UserService.Register("something@gmail.com", "ABCde1");
            serviceFactory.UserService.Login("something@gmail.com", "ABCde1");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            serviceFactory.BoardService.AddTask("something@gmail.com", "board1", "task1", "check1", new DateTime(2022, 12, 1));
            serviceFactory.TaskService.AssignTask("something@gmail.com", "board1", "backlog", 0, "something@gmail.com");

            string json = serviceFactory.TaskService.UpdateTaskDescription("something@gmail.com", "board1", "backlog", 0, null);
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result.ErrorMessage);
        }


        [Test]
        public void UpdateTaskDescriptionTest_badCase2()
        {
            // insert invalid description
            serviceFactory.UserService.Register("something@gmail.com", "ABCde1");
            serviceFactory.UserService.Login("something@gmail.com", "ABCde1");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            serviceFactory.BoardService.AddTask("something@gmail.com", "board1", "task1", "check1", new DateTime(2022, 12, 1));
            serviceFactory.TaskService.AssignTask("something@gmail.com", "board1", "backlog", 0, "something@gmail.com");

            string json = serviceFactory.TaskService.UpdateTaskDescription("something@gmail.com", "board1", "backlog", 0, "ghfhakepegreoiurtiu''iturtritueritoureiorlgnrjlhl/ghnds/klfhdsg/fjkdsghadsk/gds/jkgsdkghafdjg/hGL/JKSD;DKP'DKprrepgorogpjgi[twogjtwiohtuowtrhgot'uhger'oigheroghreogrehgirehgregiehcjgkasnckglapeotkshvkgkslclgrejhgh; ghre; jkgrh fjvfg; lskjg lgjdflkg; jsdr; gljs kvjdflk'gjsdf'l jflkgjrdglk'srjgrsd'klgshdfdlkjf'l'");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [Test]
        public void AssignTask()
        {
            // assign task that is not assigned 
            serviceFactory.UserService.Register("something@gmail.com", "ABCde1");
            serviceFactory.UserService.Login("something@gmail.com", "ABCde1");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            serviceFactory.BoardService.AddTask("something@gmail.com", "board1", "task1", "check1", new DateTime(2022, 12, 12));
           
            string json = serviceFactory.TaskService.AssignTask("something@gmail.com", "board1", "backlog", 0, "something@gmail.com");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.AreEqual(result, new Response() );
        }

        [Test]
        public void AssignTask2()
        {
            // assign task that is assigned to other user 
            serviceFactory.UserService.Register("something@gmail.com", "ABCde1");
            serviceFactory.UserService.Login("something@gmail.com", "ABCde1");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            serviceFactory.BoardService.AddTask("something@gmail.com", "board1", "task1", "check1", new DateTime(2022, 12, 12));
            serviceFactory.TaskService.AssignTask("something@gmail.com", "board1", "backlog", 0, "something@gmail.com");

            serviceFactory.UserService.Register("something2@gmail.com", "ABCde1");
            serviceFactory.BoardService.JoinBoard("something2@gmail.com", 0);
            string json = serviceFactory.TaskService.AssignTask("something@gmail.com", "board1", "backlog", 0, "something2@gmail.com");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.AreEqual(result, new Response());
        }


        [Test]
        public void AssignTaskBadCase1()
        {
            // assign task that is assigned to user that is not a member of the board

            serviceFactory.UserService.Register("something@gmail.com", "ABCde1");
            serviceFactory.UserService.Login("something@gmail.com", "ABCde1");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            serviceFactory.BoardService.AddTask("something@gmail.com", "board1", "task1", "check1", new DateTime(2022, 12, 12));
            serviceFactory.TaskService.AssignTask("something@gmail.com", "board1", "backlog", 0, "something@gmail.com");

            string json = serviceFactory.TaskService.AssignTask("something@gmail.com", "board1", "backlog", 0, "something2@gmail.com");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result.ErrorMessage);
        }

        [Test]
        public void AssignTaskBadCase2()
        {
            // try to assign a task with a user that is not the assignee

            serviceFactory.UserService.Register("something@gmail.com", "ABCde1");
            serviceFactory.UserService.Login("something@gmail.com", "ABCde1");
            serviceFactory.BoardService.AddBoard("something@gmail.com", "board1");
            serviceFactory.BoardService.AddTask("something@gmail.com", "board1", "task1", "check1", new DateTime(2022, 12, 12));
            serviceFactory.TaskService.AssignTask("something@gmail.com", "board1", "backlog", 0, "something@gmail.com");

            serviceFactory.UserService.Register("something2@gmail.com", "ABCde1");
            serviceFactory.UserService.Login("something2@gmail.com", "ABCde1");

            serviceFactory.UserService.Register("something3@gmail.com", "ABCde1");
            serviceFactory.UserService.Login("something3@gmail.com", "ABCde1");

            string json = serviceFactory.TaskService.AssignTask("something2@gmail.com", "board1", "backlog", 0, "something3@gmail.com");
            Response? result = JsonSerializer.Deserialize<Response>(json);
            Assert.IsNotNull(result.ErrorMessage);
        }
    }
}
