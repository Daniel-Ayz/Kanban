using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DAL;
using NUnit.Framework;

namespace BusinessTests
{
    public class TaskTests
    {
        Task task;

        [SetUp]
        public void Setup()
        {
            new TaskMapper().DeleteAllData();
            new BoardMapper().Insert(new BoardDTO(1, "board1", "yehuda@gmail.com"));
            task = Task.BulidTask(new DateTime(2022, 12, 12), "title", "description", 1, 1, "backLog");
            //task.AssignTask("something", "something");
            //new TaskMapper().Delete(task.TaskDTO);
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
            Task task1 = Task.BulidTask(new DateTime(2022, 11, 12), "title1", "description1", 2, 2, "backLog");
            Task task2 = Task.BulidTask(new DateTime(2022, 10, 12), "title2", "description2", 3, 3, "backLog");
            task1.AssignTask("something1", "something1");
            task2.AssignTask("something2", "something2");
            //new TaskMapper().Delete(task1.TaskDTO);
        }
        [Test]
        public void UpdateTaskDueDateTest()
        {
            task.AssignTask("something@gmail.com", "something@gmail.com");
            task.UpdateTaskDueDate( "something@gmail.com", new DateTime(2023, 10, 10));
            Assert.AreEqual(task.DueDate, new DateTime(2023, 10, 10));
        }

        [Test]
        public void UpdateTaskDueDateTest_badCase()
        {
            task.AssignTask("something@gmail.com", "something@gmail.com");
            var ex = Assert.Throws<Exception>(() => task.UpdateTaskDueDate("something@gmail.com",new DateTime(2021, 10, 10)));
            Assert.AreEqual("Error: new due date passed", ex.Message);
        }

        [Test]
        public void UpdateTaskTitleTest()
        {
            task.AssignTask("something@gmail.com", "something@gmail.com");
            task.UpdateTaskTitle("something@gmail.com", "hello");
            Assert.AreEqual(task.Title, "hello");
        }

        [Test]
        public void UpdateTaskTitleTest_badCase1()
        {
            task.AssignTask("something@gmail.com", "something@gmail.com");
            var ex = Assert.Throws<Exception>(() => task.UpdateTaskTitle("something@gmail.com", null));
            Assert.AreEqual("Error: title cannot be null", ex.Message);
            Assert.AreEqual(task.Title, "title");
        }

        [Test]
        public void UpdateTaskTitleTest_badCase2()
        {
            task.AssignTask("something@gmail.com", "something@gmail.com");
            var ex = Assert.Throws<Exception>(() => task.UpdateTaskTitle("something@gmail.com", ""));
            Assert.AreEqual("Error: title cannot be empty", ex.Message);
            Assert.AreEqual(task.Title, "title");
        }

        [Test]
        public void UpdateTaskTitleTest_badCase3()
        {
            task.AssignTask("something@gmail.com", "something@gmail.com");
            var ex = Assert.Throws<Exception>(() => task.UpdateTaskTitle("something@gmail.com", "vnnbjdklsc, vvknfdjklg gjsg vjkdfgjdjgjfjf jfjkf   gjds;lg gjh;sg;sfdhadfdgda'lfhd sjv kjfdj")); ;
            Assert.AreEqual("Error: title cannot be larger than 50 characters", ex.Message);
            Assert.AreEqual(task.Title, "title");
        }

        [Test]
        public void UpdateTaskDescriptionTest()
        {
            task.AssignTask("something@gmail.com", "something@gmail.com");
            task.UpdateTaskDescription("something@gmail.com", "hello");
            Assert.AreEqual(task.Description, "hello");
        }

        [Test]
        public void UpdateTaskDescriptionTest_badCase1()
        {
            task.AssignTask("something@gmail.com", "something@gmail.com");
            var ex = Assert.Throws<Exception>(() => task.UpdateTaskDescription("something@gmail.com", null));
            Assert.AreEqual("description cannot be null", ex.Message);
            Assert.AreEqual(task.Description, "description");
        }

        [Test]
        public void UpdateTaskDescriptionTest_badCase2()
        {
            task.AssignTask("something@gmail.com", "something@gmail.com");
            var ex = Assert.Throws<Exception>(() => task.UpdateTaskDescription ("something@gmail.com", "ghfhakepegreoiurtiu''iturtritueritoureiorlgnrjlhl/ghnds/klfhdsg/fjkdsghadsk/gds/jkgsdkghafdjg/hGL/JKSD;DKP'DKprrepgorogpjgi[twogjtwiohtuowtrhgot'uhger'oigheroghreogrehgirehgregiehcjgkasnckglapeotkshvkgkslclgrejhgh; ghre; jkgrh fjvfg; lskjg lgjdflkg; jsdr; gljs kvjdflk'gjsdf'l jflkgjrdglk'srjgrsd'klgshdfdlkjf'l'"));
            Assert.AreEqual("description cannot be larger than 300 characters", ex.Message);
            Assert.AreEqual(task.Description, "description");
        }

        [Test]
        public void AssignTask()
        {
            // check if can assign to a task that is not assigned yet
            task.AssignTask("assignTask", "assignTask");
            Assert.AreEqual(task.AssigneeEmail, "assignTask");
        }

        [Test]
        public void UnAssignTask()
        {
            // check if task is unassigned
            task.AssignTask("ugy", "ugy");
            task.UnassignTask();
            Assert.AreEqual(task.AssigneeEmail, "unassigned");
        }

        [Test]
        public void SelectAllTasks()
        {
            task.AssignTask("something@gmail.com", "something@gmail.com");
            List<TaskDTO> res = ((TaskMapper)(task.TaskDTO.Mapper)).SelectAllTasks();
            Assert.AreEqual(res[0].TaskId, 1);

        }

    }
}
