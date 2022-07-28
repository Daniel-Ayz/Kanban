using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.SQLite;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DAL;

namespace ServiceTests
{
    internal class MainTests
    {
        static void Main(string[] args)
        {
            GradingService grading = new GradingService();
            Console.WriteLine(grading.DeleteData());

            // RegisterTests(grading);
             //LoginTests(grading);
            // LogoutTests(grading);
            // AddBoardTests(grading);
            // AddTaskTests(grading);
            // GetColumnNameTests(grading);
            // GetColumnLimitTests(grading);
            // LimitColumnTests(grading);
            //AssignTaskTests(grading);
            //UpdateTaskDescriptionTests(grading);
            // UpdateTaskDueDateTests(grading);
            //UpdateTaskTitleTests(grading);
            //AdvanceTaskTests(grading);
            //GetColumnTests(grading);
            // RemoveBoardTests(grading);
            // InProgressTasksTests(grading);
            // GetUserBoardsTests(grading);
            // JoinBoardTests(grading);
            // LeaveBoardTests(grading);
            // TransferOwnershipTests(grading);

            static void RegisterTests(GradingService grading)
            {
                // legal register
                Console.WriteLine(grading.Register("Register1@email.com", "Register1"));

                // email already exists
                Console.WriteLine(grading.Register("Register1@email.com", "Register1"));

                // illegal emails
                Console.WriteLine(grading.Register("noShtroudle", "Password1"));
                Console.WriteLine(grading.Register("tooManyCharacters" +
                                                    "tooManyCharacters" +
                                                    "tooManyCharacters" +
                                                    "tooManyCharacters" +
                                                    "tooManyCharacters" +
                                                    "tooManyCharacters" +
                                                    "tooManyCharacters" +
                                                    "@email.com", "Password1"));

                // illegal password
                Console.WriteLine(grading.Register("email1@email.com", "Aa1"));
                Console.WriteLine(grading.Register("email2@email.com", "Aa1Aa1Aa1Aa1Aa1Aa1Aa1"));
                Console.WriteLine(grading.Register("email3@email.com", "password3"));
                Console.WriteLine(grading.Register("email4@email.com", "PASSWORD3"));
                Console.WriteLine(grading.Register("email5@email.com", "Password"));
            }
            static void LoginTests(GradingService grading)
            {
                grading.Register("LoginTests1@email.com", "LoginTests1");
                grading.Logout("LoginTests1@email.com");

                // legal login
                Console.WriteLine(grading.Login("LoginTests1@email.com", "LoginTests1"));

                // already login
                Console.WriteLine(grading.Login("LoginTests1@email.com", "LoginTests1"));

                // wrong password
                Console.WriteLine(grading.Login("LoginTests1@email.com", "wrongPassword1"));

                // no such user
                Console.WriteLine(grading.Login("noSuchUser@email.com", "LoginTests1"));
            }
            static void LogoutTests(GradingService grading)
            {
                grading.Register("LogoutTests1@email.com", "LogoutTests1");

                // legal logout
                Console.WriteLine(grading.Logout("LogoutTests1@email.com"));

                // already logout
                Console.WriteLine(grading.Logout("LogoutTests1@email.com"));

                // no such user
                Console.WriteLine(grading.Logout("noSuchUser@email.com"));
            }
            static void AddBoardTests(GradingService grading)
            {
                grading.Register("AddBoardTests1@email.com", "AddBoardTests1");

                // legal add board
                Console.WriteLine(grading.AddBoard("AddBoardTests1@email.com", "AddBoardTestsBoard"));

                // no such user
                Console.WriteLine(grading.AddBoard("noSuchUser@email.com", "noSuchUser"));

                // user logout
                grading.Logout("AddBoardTests1@email.com");
                Console.WriteLine(grading.AddBoard("AddBoardTests1@email.com", "userLogout"));
                grading.Login("AddBoardTests1@email.com", "AddBoardTests1");

                // same board name for same user
                Console.WriteLine(grading.AddBoard("AddBoardTests1@email.com", "AddBoardTestsBoard"));

                // legal add board (same name for different users)
                grading.Register("AddBoardTests2@email.com", "AddBoardTests2");
                Console.WriteLine(grading.AddBoard("AddBoardTests2@email.com", "AddBoardTestsBoard"));
            }
            static void AddTaskTests(GradingService grading)
            {
                grading.Register("AddTaskTests1@email.com", "AddTaskTests1");
                grading.AddBoard("AddTaskTests1@email.com", "AddTaskTestsBoard");

                // legal add task
                Console.WriteLine(grading.AddTask("AddTaskTests1@email.com", "AddTaskTestsBoard", "Title1", "desc1", new DateTime(2023, 7, 1)));

                // no such user
                Console.WriteLine(grading.AddTask("noSuchUser@email.com", "AddTaskTestsBoard", "Title1", "desc1", new DateTime(2023, 7, 1)));

                // user logout
                grading.Logout("AddTaskTests1@email.com");
                Console.WriteLine(grading.AddTask("AddTaskTests1@email.com", "AddTaskTestsBoard", "Title1", "desc1", new DateTime(2023, 7, 1)));
                grading.Login("AddTaskTests1@email.com", "AddTaskTests1");

                // no such board
                Console.WriteLine(grading.AddTask("AddTaskTests1@email.com", "noSuchBoard", "Title1", "desc1", new DateTime(2023, 7, 1)));

                // due date passed
                Console.WriteLine(grading.AddTask("AddTaskTests1@email.com", "AddTaskTestsBoard", "Title1", "desc1", new DateTime(2003, 7, 1)));
            }
            static void GetColumnNameTests(GradingService grading)
            {
                grading.Register("GetColumnNameTests1@email.com", "GetColumnNameTests1");
                grading.AddBoard("GetColumnNameTests1@email.com", "GetColumnNameTestsBoard");

                // legal get column name
                Console.WriteLine(grading.GetColumnName("GetColumnNameTests1@email.com", "GetColumnNameTestsBoard", 0));
                Console.WriteLine(grading.GetColumnName("GetColumnNameTests1@email.com", "GetColumnNameTestsBoard", 1));
                Console.WriteLine(grading.GetColumnName("GetColumnNameTests1@email.com", "GetColumnNameTestsBoard", 2));

                // no such user
                Console.WriteLine(grading.GetColumnName("noSuchUser@email.com", "GetColumnNameTestsBoard", 0));

                // user logout
                grading.Logout("GetColumnNameTests1@email.com");
                Console.WriteLine(grading.GetColumnName("GetColumnNameTests1@email.com", "GetColumnNameTestsBoard", 1));
                grading.Login("GetColumnNameTests1@email.com", "GetColumnNameTests1");

                // no such board
                Console.WriteLine(grading.GetColumnName("GetColumnNameTests1@email.com", "noSuchBoard", 2));

                // no such column
                Console.WriteLine(grading.GetColumnName("GetColumnNameTests1@email.com", "GetColumnNameTestsBoard", -1));
                Console.WriteLine(grading.GetColumnName("GetColumnNameTests1@email.com", "GetColumnNameTestsBoard", 3));
            }
            static void GetColumnLimitTests(GradingService grading)
            {
                grading.Register("GetColumnLimitTests1@email.com", "GetColumnLimitTests1");
                grading.AddBoard("GetColumnLimitTests1@email.com", "GetColumnLimitTestsBoard");

                // legal get column limit (default -1)
                Console.WriteLine(grading.GetColumnLimit("GetColumnLimitTests1@email.com", "GetColumnLimitTestsBoard", 0));
                Console.WriteLine(grading.GetColumnLimit("GetColumnLimitTests1@email.com", "GetColumnLimitTestsBoard", 1));
                Console.WriteLine(grading.GetColumnLimit("GetColumnLimitTests1@email.com", "GetColumnLimitTestsBoard", 2));

                // no such user
                Console.WriteLine(grading.GetColumnLimit("noSuchUser@email.com", "GetColumnLimitTestsBoard", 1));

                // user logout
                Console.WriteLine(grading.Logout("GetColumnLimitTests1@email.com"));
                Console.WriteLine(grading.GetColumnLimit("GetColumnLimitTests1@email.com", "GetColumnLimitTestsBoard", 2));
                Console.WriteLine(grading.Login("GetColumnLimitTests1@email.com", "GetColumnLimitTests1"));

                // no such board
                Console.WriteLine(grading.GetColumnLimit("GetColumnLimitTests1@email.com", "noSuchBoard", 2));

                // no such column
                Console.WriteLine(grading.GetColumnLimit("GetColumnLimitTests1@email.com", "GetColumnLimitTestsBoard", -1));
                Console.WriteLine(grading.GetColumnLimit("GetColumnLimitTests1@email.com", "GetColumnLimitTestsBoard", 3));
            }
            static void LimitColumnTests(GradingService grading)
            {
                grading.Register("LimitColumnTests1@email.com", "LimitColumnTests1");
                grading.AddBoard("LimitColumnTests1@email.com", "LimitColumnTestsBoard");

                // legal limit column
                Console.WriteLine(grading.LimitColumn("LimitColumnTests1@email.com", "LimitColumnTestsBoard", 0, 4));
                Console.WriteLine(grading.LimitColumn("LimitColumnTests1@email.com", "LimitColumnTestsBoard", 1, 5));
                Console.WriteLine(grading.LimitColumn("LimitColumnTests1@email.com", "LimitColumnTestsBoard", 2, 6));

                // no such user
                Console.WriteLine(grading.LimitColumn("noSuchUser@email.com", "LimitColumnTestsBoard", 0, 7));

                // user logout
                grading.Logout("LimitColumnTests1@email.com");
                Console.WriteLine(grading.LimitColumn("LimitColumnTests1@email.com", "LimitColumnTestsBoard", 0, 5));
                grading.Login("LimitColumnTests1@email.com", "LimitColumnTests1");

                // no such board
                Console.WriteLine(grading.LimitColumn("LimitColumnTests1@email.com", "noSuchBoard", 0, 5));

                // no such column
                Console.WriteLine(grading.LimitColumn("LimitColumnTests1@email.com", "LimitColumnTestsBoard", -2, 5));
                Console.WriteLine(grading.LimitColumn("LimitColumnTests1@email.com", "LimitColumnTestsBoard", 4, 5));

                // negative limitation
                Console.WriteLine(grading.LimitColumn("LimitColumnTests1@email.com", "LimitColumnTestsBoard", 0, -10));

                // limitation smaller than column size (loose tasks)
                grading.AddTask("LimitColumnTests1@email.com", "LimitColumnTestsBoard", "task1", "desc1", new DateTime(2023, 7, 1));
                grading.AddTask("LimitColumnTests1@email.com", "LimitColumnTestsBoard", "task2", "desc2", new DateTime(2023, 7, 1));
                grading.AddTask("LimitColumnTests1@email.com", "LimitColumnTestsBoard", "task3", "desc3", new DateTime(2023, 7, 1));
                Console.WriteLine(grading.LimitColumn("LimitColumnTests1@email.com", "LimitColumnTestsBoard", 0, 2));
            }
            static void AssignTaskTests(GradingService grading)
            {
                grading.Register("AssignTaskTests1@email.com", "AssignTaskTests1");
                grading.AddBoard("AssignTaskTests1@email.com", "AssignTaskTestsBoard");
                grading.AddTask("AssignTaskTests1@email.com", "AssignTaskTestsBoard", "Title1", "desc1", new DateTime(2025, 2, 3));

                // legal assign task from boardOwner to himself
                Console.WriteLine(grading.AssignTask("AssignTaskTests1@email.com", "AssignTaskTestsBoard", 0, 0, "AssignTaskTests1@email.com"));

                // already assign
                Console.WriteLine(grading.AssignTask("AssignTaskTests1@email.com", "AssignTaskTestsBoard", 0, 0, "AssignTaskTests1@email.com"));

                // no such user
                Console.WriteLine(grading.AssignTask("noSuchUser@email.com", "AssignTaskTestsBoard", 0, 0, "AssignTaskTests1@email.com"));
                Console.WriteLine(grading.AssignTask("AssignTaskTests1@email.com", "AssignTaskTestsBoard", 0, 0, "noSuchUser@email.com"));

                // user is not board member
                grading.Register("AssignTaskTests2@email.com", "AssignTaskTests2");
                Console.WriteLine(grading.AssignTask("AssignTaskTests2@email.com", "AssignTaskTestsBoard", 0, 0, "AssignTaskTests1@email.com"));
                Console.WriteLine(grading.AssignTask("AssignTaskTests1@email.com", "AssignTaskTestsBoard", 0, 0, "AssignTaskTests2@email.com"));

                // legal assign task to other board member
                grading.JoinBoard("AssignTaskTests2@email.com", 1);
                Console.WriteLine(grading.AssignTask("AssignTaskTests1@email.com", "AssignTaskTestsBoard", 0, 0, "AssignTaskTests2@email.com"));
                Console.WriteLine(grading.AssignTask("AssignTaskTests2@email.com", "AssignTaskTestsBoard", 0, 0, "AssignTaskTests1@email.com"));

                // user logout
                grading.Logout("AssignTaskTests1@email.com");
                Console.WriteLine(grading.AssignTask("AssignTaskTests1@email.com", "AssignTaskTestsBoard", 0, 0, "AssignTaskTests2@email.com"));
                grading.Login("AssignTaskTests1@email.com", "AssignTaskTests1");

                // no such board
                Console.WriteLine(grading.AssignTask("AssignTaskTests1@email.com", "noSuchBoard", 0, 0, "AssignTaskTests2@email.com"));

                // no such column
                Console.WriteLine(grading.AssignTask("AssignTaskTests1@email.com", "AssignTaskTestsBoard", 3, 0, "AssignTaskTests2@email.com"));

                // no such task
                Console.WriteLine(grading.AssignTask("AssignTaskTests1@email.com", "AssignTaskTestsBoard", 0, 400, "AssignTaskTests2@email.com"));

            }
            static void UpdateTaskDescriptionTests(GradingService grading)
            {
                grading.Register("UpdateTaskDescriptionTests1@email.com", "Description1");
                grading.AddBoard("UpdateTaskDescriptionTests1@email.com", "UpdateTaskDescriptionTestsBoard");
                grading.AddTask("UpdateTaskDescriptionTests1@email.com", "UpdateTaskDescriptionTestsBoard", "Title1", "desc1", new DateTime(2025, 2, 3));

                // no assignee
                Console.WriteLine(grading.UpdateTaskDescription("UpdateTaskDescriptionTests1@email.com", "UpdateTaskDescriptionTestsBoard", 0, 0, "newDescription"));

                // legal update description
                grading.AssignTask("UpdateTaskDescriptionTests1@email.com", "UpdateTaskDescriptionTestsBoard", 0, 0, "UpdateTaskDescriptionTests1@email.com");
                Console.WriteLine(grading.UpdateTaskDescription("UpdateTaskDescriptionTests1@email.com", "UpdateTaskDescriptionTestsBoard", 0, 0, "newDescription"));

                // no such user
                Console.WriteLine(grading.UpdateTaskDescription("noSuchUser@email.com", "UpdateTaskDescriptionTestsBoard", 0, 0, "newDescription1"));

                // user logout
                grading.Logout("UpdateTaskDescriptionTests1@email.com");
                Console.WriteLine(grading.UpdateTaskDescription("UpdateTaskDescriptionTests1@email.com", "UpdateTaskDescriptionTestsBoard", 0, 0, "newDescription1"));
                grading.Login("UpdateTaskDescriptionTests1@email.com", "Description1");

                // no such board
                Console.WriteLine(grading.UpdateTaskDescription("UpdateTaskDescriptionTests1@email.com", "noSuchBoard", 0, 0, "newDescription1"));

                // no such column
                Console.WriteLine(grading.UpdateTaskDescription("UpdateTaskDescriptionTests1@email.com", "UpdateTaskDescriptionTestsBoard", -1, 0, "newDescription1"));

                // no such task
                Console.WriteLine(grading.UpdateTaskDescription("UpdateTaskDescriptionTests1@email.com", "UpdateTaskDescriptionTestsBoard", 0, 123, "newDescription1"));

                // user is not the task assignee
                grading.Register("UpdateTaskDescriptionTests2@email.com", "Description2");
                grading.JoinBoard("UpdateTaskDescriptionTests2@email.com", 1);
                Console.WriteLine(grading.UpdateTaskDescription("UpdateTaskDescriptionTests2@email.com", "UpdateTaskDescriptionTestsBoard", 0, 0, "newDescription1"));
            }
            static void UpdateTaskDueDateTests(GradingService grading)
            {
                grading.Register("UpdateTaskDueDateTests1@email.com", "UpdateDueDate1");
                grading.AddBoard("UpdateTaskDueDateTests1@email.com", "UpdateTaskDueDateTestsBoard");
                grading.AddTask("UpdateTaskDueDateTests1@email.com", "UpdateTaskDueDateTestsBoard", "Title1", "desc1", new DateTime(2025, 2, 3));

                // no assignee
                Console.WriteLine(grading.UpdateTaskDueDate("UpdateTaskDueDateTests1@email.com", "UpdateTaskDueDateTestsBoard", 0, 0, new DateTime(2026, 4, 9)));

                // legal update due date
                grading.AssignTask("UpdateTaskDueDateTests1@email.com", "UpdateTaskDueDateTestsBoard", 0, 0, "UpdateTaskDueDateTests1@email.com");
                Console.WriteLine(grading.UpdateTaskDueDate("UpdateTaskDueDateTests1@email.com", "UpdateTaskDueDateTestsBoard", 0, 0, new DateTime(2026, 4, 9)));

                // no such user
                Console.WriteLine(grading.UpdateTaskDueDate("noSuchUser@email.com", "UpdateTaskDueDateTestsBoard", 0, 0, new DateTime(2028, 5, 1)));

                // user logout
                grading.Logout("UpdateTaskDueDateTests1@email.com");
                Console.WriteLine(grading.UpdateTaskDueDate("UpdateTaskDueDateTests1@email.com", "UpdateTaskDueDateTestsBoard", 0, 0, new DateTime(2028, 5, 1)));
                grading.Login("UpdateTaskDueDateTests1@email.com", "UpdateDueDate1");

                // no such board
                Console.WriteLine(grading.UpdateTaskDueDate("UpdateTaskDueDateTests1@email.com", "noSuchBoard", 0, 0, new DateTime(2028, 5, 1)));

                // no such column
                Console.WriteLine(grading.UpdateTaskDueDate("UpdateTaskDueDateTests1@email.com", "UpdateTaskDueDateTestsBoard", 3, 0, new DateTime(2028, 5, 1)));

                // no such task
                Console.WriteLine(grading.UpdateTaskDueDate("UpdateTaskDueDateTests1@email.com", "UpdateTaskDueDateTestsBoard", 0, 2345, new DateTime(2028, 5, 1)));

                // user is not the task assignee
                grading.Register("UpdateTaskDueDateTests2@email.com", "UpdateDueDate2");
                grading.JoinBoard("UpdateTaskDueDateTests2@email.com", 1);
                Console.WriteLine(grading.UpdateTaskDueDate("UpdateTaskDueDateTests2@email.com", "UpdateTaskDueDateTestsBoard", 0, 0, new DateTime(2028, 5, 1)));

            }
            static void UpdateTaskTitleTests(GradingService grading)
            {
                grading.Register("UpdateTaskTitleTests1@email.com", "UpdateTitle1");
                grading.AddBoard("UpdateTaskTitleTests1@email.com", "UpdateTaskTitleTestsBoard");
                grading.AddTask("UpdateTaskTitleTests1@email.com", "UpdateTaskTitleTestsBoard", "Title1", "desc1", new DateTime(2025, 2, 3));

                // no assignee
                Console.WriteLine(grading.UpdateTaskTitle("UpdateTaskTitleTests1@email.com", "UpdateTaskTitleTestsBoard", 0, 0, "newTitle"));

                // legal update description
                grading.AssignTask("UpdateTaskTitleTests1@email.com", "UpdateTaskTitleTestsBoard", 0, 0, "UpdateTaskTitleTests1@email.com");
                Console.WriteLine(grading.UpdateTaskTitle("UpdateTaskTitleTests1@email.com", "UpdateTaskTitleTestsBoard", 0, 0, "newTitle"));

                // no such user
                Console.WriteLine(grading.UpdateTaskTitle("noSuchUser@email.com", "UpdateTaskTitleTestsBoard", 0, 0, "newTitle1"));

                // user logout
                grading.Logout("UpdateTaskTitleTests1@email.com");
                Console.WriteLine(grading.UpdateTaskTitle("UpdateTaskTitleTests1@email.com", "UpdateTaskTitleTestsBoard", 0, 0, "newTitle1"));
                grading.Login("UpdateTaskTitleTests1@email.com", "UpdateTitle1");

                // no such board
                Console.WriteLine(grading.UpdateTaskTitle("UpdateTaskTitleTests1@email.com", "noSuchBoard", 0, 0, "newTitle1"));

                // no such column
                Console.WriteLine(grading.UpdateTaskTitle("UpdateTaskTitleTests1@email.com", "UpdateTaskTitleTestsBoard", 4, 0, "newTitle1"));

                // no such task
                Console.WriteLine(grading.UpdateTaskTitle("UpdateTaskTitleTests1@email.com", "UpdateTaskTitleTestsBoard", 0, 300, "newTitle1"));

                // user is not the task assignee
                grading.Register("UpdateTaskTitleTests2@email.com", "UpdateTitle2");
                grading.JoinBoard("UpdateTaskTitleTests2@email.com", 1);
                Console.WriteLine(grading.UpdateTaskTitle("UpdateTaskTitleTests2@email.com", "UpdateTaskTitleTestsBoard", 0, 0, "newTitle1"));

            }
            static void AdvanceTaskTests(GradingService grading)
            {
                grading.Register("AdvanceTaskTests1@email.com", "AdvanceTaskTests1");
                grading.AddBoard("AdvanceTaskTests1@email.com", "AdvanceTaskTestsBoard");
                grading.AddTask("AdvanceTaskTests1@email.com", "AdvanceTaskTestsBoard", "Title1", "desc1", new DateTime(2025, 2, 3));
                grading.AssignTask("AdvanceTaskTests1@email.com", "AdvanceTaskTestsBoard", 0, 0, "AdvanceTaskTests1@email.com");

                // legal advance task
                Console.WriteLine(grading.AdvanceTask("AdvanceTaskTests1@email.com", "AdvanceTaskTestsBoard", 0, 0));
                Console.WriteLine(grading.AdvanceTask("AdvanceTaskTests1@email.com", "AdvanceTaskTestsBoard", 1, 0));

                // no such movement
                Console.WriteLine(grading.AdvanceTask("AdvanceTaskTests1@email.com", "AdvanceTaskTestsBoard", 2, 0));

                // no such user
                grading.AddTask("AdvanceTaskTests1@email.com", "AdvanceTaskTestsBoard", "Title2", "desc2", new DateTime(2026, 2, 3));
                Console.WriteLine(grading.AdvanceTask("noSuchUser@email.com", "AdvanceTaskTestsBoard", 0, 1));

                // user logout
                grading.Logout("AdvanceTaskTests1@email.com");
                Console.WriteLine(grading.AdvanceTask("AdvanceTaskTests1@email.com", "AdvanceTaskTestsBoard", 0, 1));
                grading.Login("AdvanceTaskTests1@email.com", "AdvanceTaskTests1");

                // no such board
                Console.WriteLine(grading.AdvanceTask("AdvanceTaskTests1@email.com", "noSuchBoard", 0, 1));

                // no such column
                Console.WriteLine(grading.AdvanceTask("AdvanceTaskTests1@email.com", "AdvanceTaskTestsBoard", 3, 1));

                // no such task
                Console.WriteLine(grading.AdvanceTask("AdvanceTaskTests1@email.com", "AdvanceTaskTestsBoard", 0, 543));

                // user is not the task assignee
                grading.Register("AdvanceTaskTests2@email.com", "AdvanceTaskTests2");
                grading.JoinBoard("AdvanceTaskTests2@email.com", 1);
                Console.WriteLine(grading.AdvanceTask("AdvanceTaskTests2@email.com", "AdvanceTaskTestsBoard", 0, 1));
            }
            static void GetColumnTests(GradingService grading)
            {
                grading.Register("GetColumnTests1@email.com", "GetColumnTests1");
                grading.AddBoard("GetColumnTests1@email.com", "GetColumnTestsBoard");
                grading.AddTask("GetColumnTests1@email.com", "GetColumnTestsBoard", "Title1", "desc1", new DateTime(2023, 2, 3));
                grading.AddTask("GetColumnTests1@email.com", "GetColumnTestsBoard", "Title2", "desc2", new DateTime(2024, 2, 3));
                grading.AddTask("GetColumnTests1@email.com", "GetColumnTestsBoard", "Title3", "desc3", new DateTime(2025, 2, 3));
                grading.AdvanceTask("GetColumnTests1@email.com", "GetColumnTestsBoard", 0, 1);
                grading.AdvanceTask("GetColumnTests1@email.com", "GetColumnTestsBoard", 0, 2);
                grading.AdvanceTask("GetColumnTests1@email.com", "GetColumnTestsBoard", 1, 2);

                // legal get column
                Console.WriteLine(grading.GetColumn("GetColumnTests1@email.com", "GetColumnTestsBoard", 0));
                Console.WriteLine(grading.GetColumn("GetColumnTests1@email.com", "GetColumnTestsBoard", 1));
                Console.WriteLine(grading.GetColumn("GetColumnTests1@email.com", "GetColumnTestsBoard", 2));

                // no such user
                Console.WriteLine(grading.GetColumn("noSuchUser@email.com", "GetColumnTestsBoard", 0));

                // user logout
                grading.Logout("GetColumnTests1@email.com");
                Console.WriteLine(grading.GetColumn("GetColumnTests1@email.com", "GetColumnTestsBoard", 1));
                grading.Login("GetColumnTests1@email.com", "GetColumnTests1");

                // no such board
                Console.WriteLine(grading.GetColumn("GetColumnTests1@email.com", "noSuchBoard", 2));

                // no such column
                Console.WriteLine(grading.GetColumn("GetColumnTests1@email.com", "GetColumnTestsBoard", 4));

            }
            static void RemoveBoardTests(GradingService grading)
            {
                grading.Register("RemoveBoardTests1@email.com", "RemoveBoardTests1");
                grading.AddBoard("RemoveBoardTests1@email.com", "RemoveBoardTestsBoard");

                // legal remove board
                Console.WriteLine(grading.RemoveBoard("RemoveBoardTests1@email.com", "RemoveBoardTestsBoard"));

                // no such user
                grading.AddBoard("RemoveBoardTests1@email.com", "RemoveBoardTestsBoard2");
                Console.WriteLine(grading.RemoveBoard("noSuchUser@email.com", "RemoveBoardTestsBoard1"));

                // user logout
                grading.Logout("RemoveBoardTests1@email.com");
                Console.WriteLine(grading.RemoveBoard("RemoveBoardTests1@email.com", "RemoveBoardTestsBoard1"));
                grading.Login("RemoveBoardTests1@email.com", "RemoveBoardTests1");

                // no such board
                Console.WriteLine(grading.RemoveBoard("RemoveBoardTests1@email.com", "noSuchBoard"));

                // user is not board owner
                grading.Register("RemoveBoardTests2@email.com", "RemoveBoardTests2");
                Console.WriteLine(grading.RemoveBoard("RemoveBoardTests2@email.com", "RemoveBoardTestsBoard1"));
            }
            static void InProgressTasksTests(GradingService grading)
            {
                grading.Register("InProgressTasksTests1@email.com", "InProgress1");
                grading.AddBoard("InProgressTasksTests1@email.com", "InProgressTasksTestsBoard");
                grading.AddTask("InProgressTasksTests1@email.com", "InProgressTasksTestsBoard", "Title1", "desc1", new DateTime(2023, 2, 3));
                grading.AddTask("InProgressTasksTests1@email.com", "InProgressTasksTestsBoard", "Title2", "desc2", new DateTime(2024, 2, 3));
                grading.AddTask("InProgressTasksTests1@email.com", "InProgressTasksTestsBoard", "Title3", "desc3", new DateTime(2025, 2, 3));
                grading.AssignTask("InProgressTasksTests1@email.com", "InProgressTasksTestsBoard", 0, 0,
                    "InProgressTasksTests1@email.com");
                grading.AssignTask("InProgressTasksTests1@email.com", "InProgressTasksTestsBoard", 0, 1,
                    "InProgressTasksTests1@email.com");
                grading.AssignTask("InProgressTasksTests1@email.com", "InProgressTasksTestsBoard", 0, 2,
                    "InProgressTasksTests1@email.com");
                grading.AdvanceTask("InProgressTasksTests1@email.com", "InProgressTasksTestsBoard", 0, 0);
                grading.AdvanceTask("InProgressTasksTests1@email.com", "InProgressTasksTestsBoard", 0, 1);
                grading.AdvanceTask("InProgressTasksTests1@email.com", "InProgressTasksTestsBoard", 0, 2);

                // legal in progress task
                Console.WriteLine(grading.InProgressTasks("InProgressTasksTests1@email.com"));

                // no such user
                Console.WriteLine(grading.InProgressTasks("noSuchUser@email.com"));

                // user logout
                grading.Logout("InProgressTasksTests1@email.com");
                Console.WriteLine(grading.InProgressTasks("InProgressTasksTests1@email.com"));
            }
            static void GetUserBoardsTests(GradingService grading)
            {
                grading.Register("GetUserBoardsTests1@email.com", "GetUserBoardsTests1");
                grading.AddBoard("GetUserBoardsTests1@email.com", "GetUserBoardsTestsBoard1");
                grading.AddBoard("GetUserBoardsTests1@email.com", "GetUserBoardsTestsBoard2");
                grading.AddBoard("GetUserBoardsTests1@email.com", "GetUserBoardsTestsBoard3");

                // legal get user boards
                Console.WriteLine(grading.GetUserBoards("GetUserBoardsTests1@email.com"));

                // no such user
                Console.WriteLine(grading.GetUserBoards("noSuchUser@email.com"));

                // user logout
                grading.Logout("GetUserBoardsTests1@email.com");
                Console.WriteLine(grading.GetUserBoards("GetUserBoardsTests1@email.com"));
            }
            static void JoinBoardTests(GradingService grading)
            {
                grading.Register("JoinBoardTests1@email.com", "JoinBoardTests1");
                grading.AddBoard("JoinBoardTests1@email.com", "JoinBoardTestsBoard");

                // legal join board
                grading.Register("JoinBoardTests2@email.com", "JoinBoardTests2");
                Console.WriteLine(grading.JoinBoard("JoinBoardTests2@email.com", 1));

                // already board member
                Console.WriteLine(grading.JoinBoard("JoinBoardTests2@email.com", 1));

                // no such user
                Console.WriteLine(grading.JoinBoard("noSuchUser@email.com", 1));

                // user logout
                grading.Register("JoinBoardTests3@email.com", "JoinBoardTests3");
                grading.Logout("JoinBoardTests3@email.com");
                Console.WriteLine(grading.JoinBoard("JoinBoardTests3@email.com", 1));

                // no such board
                Console.WriteLine(grading.JoinBoard("JoinBoardTests2@email.com", 2));
            }
            static void LeaveBoardTests(GradingService grading)
            {
                grading.Register("LeaveBoardTests1@email.com", "LeaveBoardTests1");
                grading.AddBoard("LeaveBoardTests1@email.com", "LeaveBoardTestsBoard");
                grading.Register("JoinBoardTests2@email.com", "JoinBoardTests2");
                grading.JoinBoard("JoinBoardTests2@email.com", 1);

                // legal leave board
                Console.WriteLine(grading.LeaveBoard("JoinBoardTests2@email.com", 1));

                // board owner cannot leave a board
                Console.WriteLine(grading.LeaveBoard("LeaveBoardTests1@email.com", 1));

                // not a board member
                Console.WriteLine(grading.LeaveBoard("JoinBoardTests2@email.com", 1));

                // no such user
                Console.WriteLine(grading.LeaveBoard("noSuchUser@email.com", 1));

                // user logout
                grading.JoinBoard("JoinBoardTests2@email.com", 1);
                grading.Logout("JoinBoardTests2@email.com");
                Console.WriteLine(grading.LeaveBoard("JoinBoardTests2@email.com", 1));
                grading.Login("JoinBoardTests2@email.com", "JoinBoardTests2");

                // no such board
                Console.WriteLine(grading.JoinBoard("JoinBoardTests2@email.com", 2));
            }
            static void TransferOwnershipTests(GradingService grading)
            {
                grading.Register("TransferOwnershipTests1@email.com", "Transfer1");
                grading.AddBoard("TransferOwnershipTests1@email.com", "TransferOwnershipTestsBoard");
                grading.Register("TransferOwnershipTests2@email.com", "Transfer2");
                grading.JoinBoard("TransferOwnershipTests2@email.com", 1);

                // legal transfer ownership
                Console.WriteLine(grading.TransferOwnership("TransferOwnershipTests1@email.com", "TransferOwnershipTests2@email.com", "TransferOwnershipTestsBoard"));

                // not a board member
                grading.Register("TransferOwnershipTests3@email.com", "Transfer3");
                Console.WriteLine(grading.TransferOwnership("TransferOwnershipTests2@email.com", "TransferOwnershipTests3@email.com", "TransferOwnershipTestsBoard"));

                // not the board owner
                grading.JoinBoard("TransferOwnershipTests3@email.com", 1);
                Console.WriteLine(grading.TransferOwnership("TransferOwnershipTests1@email.com", "TransferOwnershipTests3@email.com", "TransferOwnershipTestsBoard"));

                // no such user
                Console.WriteLine(grading.TransferOwnership("noSuchUser@email.com", "TransferOwnershipTests3@email.com", "TransferOwnershipTestsBoard"));
                Console.WriteLine(grading.TransferOwnership("TransferOwnershipTests2@email.com", "noSuchUser@email.com", "TransferOwnershipTestsBoard"));

                // user logout
                grading.Logout("TransferOwnershipTests2@email.com");
                Console.WriteLine(grading.TransferOwnership("TransferOwnershipTests2@email.com", "TransferOwnershipTests3@email.com", "TransferOwnershipTestsBoard"));
                grading.Login("TransferOwnershipTests2@email.com", "Transfer2");

                // no such board
                Console.WriteLine(grading.TransferOwnership("TransferOwnershipTests2@email.com", "TransferOwnershipTests3@email.com", "noSuchBoard"));
            }

        }
    }
}
