
using IntroSE.Kanban.Backend.BusinessLayer;
using NUnit.Framework;
using System;
using IntroSE.Kanban.Backend.DAL;

namespace BusinessTests
{
    public class ColumnTests
    {
        Column column;
        Task task1;
        Task task2;
        Task task3;
        Task task4;
        Task task5;
        [SetUp]
        public void Setup()
        {
            new TaskMapper().DeleteAllData();
            new ColumnMapper().DeleteAllData();
            column = Column.BuildColumn("backlog", 0);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            new BoardMapper().DeleteAllData();
            new UserMapper().DeleteAllData();
        }

        [Test]
        public void AddTaskTest()
        {
            column.AddTask("title", new DateTime(2022, 12, 12), "description", 0, 0);
            Assert.AreEqual("unassigned", column.GetTask(0).AssigneeEmail);
        }

        [Test]
        public void AddTaskTestFail()
        {
            column.SetLimit(3);
            column.AddTask("title1", new DateTime(2022, 12, 12), "description", 0, 0);
            column.AddTask("title2", new DateTime(2022, 12, 12), "description", 1, 0);
            column.AddTask("title3", new DateTime(2022, 12, 12), "description", 2, 0);
            
            Assert.Catch<Exception>(() => column.AddTask("title4", new DateTime(2022, 12, 12), "description", 3, 0));
        }

        [Test]
        public void SetLimitTest()
        {
            column.AddTask("title", new DateTime(2022, 12, 12), "description", 0, 0);
            column.SetLimit(3);
        }

        [Test]
        public void SetLimitTestFail()
        {
            column.AddTask("title1", new DateTime(2022, 12, 12), "description", 0, 0);
            column.AddTask("title2", new DateTime(2022, 12, 12), "description", 1, 0);
            column.AddTask("title3", new DateTime(2022, 12, 12), "description", 2, 0);
            Assert.Catch<Exception>(() => column.SetLimit(2));
        }
        /*
        [Test]
        public void ColumnFunctionalityTest()
        {
            column.AddTask(task1);
            column.AddTask(task2);
            column.AddTask(task3);
            column.SetLimit(3);
            column.DeleteTask(1);
            column.AddTask(task4);
            Assert.AreEqual(task4, column.GetTask(4));
        }*/

    }
}
