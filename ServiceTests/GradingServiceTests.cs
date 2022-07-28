using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;
using System.Reflection;

namespace ServiceTests
{
    [TestFixture]
    public class GradingServiceTests
    {
        GradingService service;
        string emailUser1;
        string emailUser2;
        string emailUser3;
        string boardName1;
        string boardName2;
        string boardName3;
        string password;
        string taskTitle1;
        string taskTitle2;
        string taskTitle3;
        string taskTitle4;
        string taskTitle5;
        string taskTitle6;
        string taskDescription;
        DateTime dueDate;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            service = new GradingService();
            emailUser1 = "Yehuda@gmail.com";
            emailUser2 = "Achiya@gmail.com";
            emailUser3 = "Lior@gmail.com";
            boardName1 = "BGUCourseBoard";
            boardName2 = "KanbanProjectBoard";
            boardName3 = "DragonsBoard";
            password = "Password123";
            taskTitle1 = "WriteCourseProject";
            taskTitle2 = "ReviewML1Business";
            taskTitle3 = "ReviewML2DAL";
            taskTitle4 = "ReviewML3GUI";
            taskTitle5 = "GiveGradesToTheClass";
            taskTitle6 = "InProgressAnotheBoard";
            taskDescription = "SuperSmartPlanToDo";
            dueDate = new DateTime(2022, 7, 3);
        }

        [SetUp]
        public void Setup()
        {
            service.DeleteData();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            service.DeleteData();
        }

        [Test]
        public void SingleUserSetUp()
        {
            string json1 = service.Register(emailUser1, password);
            string json2 = service.AddBoard(emailUser1, boardName1);
            string json3 = service.AddTask(emailUser1, boardName1, taskTitle1, taskDescription, dueDate);
            string json4 = service.AssignTask(emailUser1, boardName1, 0, 0, emailUser1);
            string json5 = service.AdvanceTask(emailUser1, boardName1, 0, 0);
            string json6 = service.InProgressTasks(emailUser1);
            string json7 = service.UpdateTaskDueDate(emailUser1, boardName1, 1, 0, new DateTime(2022, 9, 5));
            string json8 = service.UpdateTaskDescription(emailUser1, boardName1, 1, 0, "BetterPlanDescription");
            string json9 = service.UpdateTaskTitle(emailUser1, boardName1, 1, 0, "newTitleForTheNewPlan");
            string json95 = service.InProgressTasks(emailUser1);
            string json10 = service.GetColumnLimit(emailUser1, boardName1, 2);
            string json11 = service.LimitColumn(emailUser1, boardName1, 2, 1);
            string json12 = service.GetColumnLimit(emailUser1, boardName1, 2);
            string json13 = service.AdvanceTask(emailUser1, boardName1, 1, 0);
            string json14 = service.Logout(emailUser1);
        }

        [Test]
        public void MultipleUserSetUp()
        {
            //register 3 user
            string json1 = service.Register(emailUser1, password);
            string json2 = service.Register(emailUser2, password);
            string json3 = service.Register(emailUser3, password);
            //add 3 boards
            string json4 = service.AddBoard(emailUser1, boardName1);
            string json5 = service.AddBoard(emailUser1, boardName2);
            string json6 = service.AddBoard(emailUser2, boardName3);
            //add 3 task to boardName1
            string json7 = service.AddTask(emailUser1, boardName1, taskTitle1, taskDescription, dueDate);
            string json8 = service.AddTask(emailUser1, boardName1, taskTitle2, taskDescription, dueDate);
            string json9 = service.AddTask(emailUser1, boardName1, taskTitle3, taskDescription, dueDate);
            //add 3 task to boardName2
            string json10 = service.AddTask(emailUser1, boardName2, taskTitle6, taskDescription, dueDate);
            //assign task1, task2 to user1 on board1 && task6 on board2
            string json11 = service.AssignTask(emailUser1, boardName1, 0, 0, emailUser1);
            string json12 = service.AssignTask(emailUser1, boardName1, 0, 1, emailUser1);
            string json122 = service.AssignTask(emailUser1, boardName2, 0, 0, emailUser1);
            //get boardId of board and choose boardId of BoardName1
            int boardId;
            int boardId1 = (int)(JsonSerializer.Deserialize<int[]>((JsonElement)(JsonSerializer.Deserialize<Response>(service.GetUserBoards(emailUser1))).ReturnValue)).GetValue(0);
            int boardId2 = (int)(JsonSerializer.Deserialize<int[]>((JsonElement)(JsonSerializer.Deserialize<Response>(service.GetUserBoards(emailUser1))).ReturnValue)).GetValue(1);
            string boardNameRecieved = (string)JsonSerializer.Deserialize<string>((JsonElement)(JsonSerializer.Deserialize<Response>(service.GetBoardName(boardId1)).ReturnValue));
            if (boardNameRecieved.Equals(boardName1))
                boardId = boardId1;
            else
                boardId = boardId2;
            //join boardName1
            string json13 = service.JoinBoard(emailUser2, boardId); //join2
            string json14 = service.JoinBoard(emailUser3, boardId); //join3
            //add tasks for user2 and user3 in boardName1
            string json15 = service.AddTask(emailUser2, boardNameRecieved, taskTitle4, taskDescription, dueDate); //add task4 to user2
            string json16 = service.AddTask(emailUser3, boardNameRecieved, taskTitle5, taskDescription, dueDate); //add task5 to user3
            //advance task1->done && task2-> in progress && task6 -> in progress
            string json17 = service.AdvanceTask(emailUser1, boardNameRecieved, 0, 0); //task1-> in progress
            string json18 = service.AdvanceTask(emailUser1, boardNameRecieved, 1, 0); //task1-> done
            string json19 = service.AdvanceTask(emailUser1, boardNameRecieved, 0, 1); //task2-> in progress
            string json20 = service.AdvanceTask(emailUser1, boardName2, 0, 0);  //task6 -> in progress
            //get all user1 task in progress (task2 & task6)
            string json100 = service.InProgressTasks(emailUser1); //2 tasks
            //TaskToSend taskRecieved1 = (TaskToSend)(JsonSerializer.Deserialize<TaskToSend>((JsonElement)(JsonSerializer.Deserialize<Response>(json100).ReturnValue)));
            //TaskToSend taskRecieved2 = (TaskToSend)(JsonSerializer.Deserialize<TaskToSend>((JsonElement)(JsonSerializer.Deserialize<Response>(json100).ReturnValue)));
            //updateTask2
            string json21 = service.UpdateTaskDueDate(emailUser1, boardNameRecieved, 1, 1, new DateTime(2029, 9, 5));
            string json22 = service.UpdateTaskDescription(emailUser1, boardNameRecieved, 1, 1, "BetterPlanDescription");
            string json23 = service.UpdateTaskTitle(emailUser1, boardNameRecieved, 1, 1, "newTitleForTheNewPlan");
            // logout
            string json24 = service.Logout(emailUser1);
            string json25 = service.Logout(emailUser2);
            string json26 = service.Logout(emailUser3);
            // login
            string json27 = service.Login(emailUser1, password);
            string json28 = service.Login(emailUser2, password);
            string json29 = service.Login(emailUser3, password);
            //transfer ownership from user1 to user3
            string json30 = service.TransferOwnership(emailUser1, emailUser3, boardNameRecieved);
            //leaveboard user1
            string json31 = service.LeaveBoard(emailUser1, boardId);



            //loadata
            string json32 = service.LoadData();
            //DATA RESETED
            string json33 = service.Login(emailUser1, password);
            string json34 = service.Login(emailUser2, password);
            string json35 = service.Login(emailUser3, password);
            int boardLoadedId1 = (int)(JsonSerializer.Deserialize<int[]>((JsonElement)(JsonSerializer.Deserialize<Response>(service.GetUserBoards(emailUser1))).ReturnValue)).GetValue(0);
            int boardLoadedId2 = (int)(JsonSerializer.Deserialize<int[]>((JsonElement)(JsonSerializer.Deserialize<Response>(service.GetUserBoards(emailUser2))).ReturnValue)).GetValue(0);
            int boardLoadedId3 = (int)(JsonSerializer.Deserialize<int[]>((JsonElement)(JsonSerializer.Deserialize<Response>(service.GetUserBoards(emailUser3))).ReturnValue)).GetValue(0);
            //manipulate the data after load

            //assign tasks 2->email2 | 3,4->email3
            string json36 = service.AssignTask(emailUser2, boardName1, 0, 2, emailUser2);
            string json37 = service.AssignTask(emailUser2, boardName1, 0, 3, emailUser3);
            string json38 = service.AssignTask(emailUser3, boardName1, 0, 4, emailUser3);
            //move tasksTo in progress
            string json39 = service.AdvanceTask(emailUser2, boardName1, 0, 2); //task3-> in progress
            string json40 = service.AdvanceTask(emailUser3, boardName1, 0, 3); //task4-> in progress
            string json41 = service.AdvanceTask(emailUser3, boardName1, 0, 4); //task5-> in progress
            //get tasks in progress of all
            string json101 = service.InProgressTasks(emailUser1); //1 tasks
            string json102 = service.InProgressTasks(emailUser2); //1 tasks
            string json103 = service.InProgressTasks(emailUser3); //2 tasks
            //leave board user2
            string json42 = service.LeaveBoard(emailUser2, boardLoadedId3);
            //assign all tasks to user3 
            string json43 = service.AssignTask(emailUser3, boardName1, 1, 1, emailUser3);
            string json44 = service.AssignTask(emailUser3, boardName1, 1, 2, emailUser3);
            //and move to done
            string json45 = service.AdvanceTask(emailUser3, boardName1, 1, 1); //task2-> done
            string json46 = service.AdvanceTask(emailUser3, boardName1, 1, 2); //task3-> done
            string json47 = service.AdvanceTask(emailUser3, boardName1, 1, 3); //task4-> done
            string json48 = service.AdvanceTask(emailUser3, boardName1, 1, 4); //task5-> done
            //remove board
            string json49 = service.RemoveBoard(emailUser3, boardName1);
            //LoadData secondtime
            string json50 = service.Login(emailUser3, password);
            string json51 = service.GetUserBoards(emailUser3);

        }

    }
}

   


