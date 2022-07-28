using System;
using System.Collections.Generic;
using System.Text.Json;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardService
    {
        private BusinessLayer.BoardController boardController;
        private BusinessLayer.UserController userController;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal BoardService(BusinessLayer.BoardController bc, BusinessLayer.UserController uc)
        {
            boardController = bc;
            userController = uc;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        /// <summary>
        /// This method adds a board to a specific user.
        /// </summary>
        /// <param name="email"> Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the new board</param>
        /// <returns>The string "{}",unless an error occurs, then returns a string contains the error occured</returns>
        public string AddBoard(string email, string boardName)
        {
            try
            {
                if (userController.IsLoggedIn(email))
                {
                    boardController.AddBoard(email, boardName);
                    log.Info($"added board: {boardName} to {email}");
                    return JsonSerializer.Serialize(new Response());
                }
                else
                {
                    log.Warn($"{email} is not logged In when trying to Add {boardName}");
                    return JsonSerializer.Serialize(new Response("Error: user isn't logged in"));
                }
            }
            catch (Exception ex)
            {
                log.Warn($"thrown exception: {ex.Message}, while trying to Add {boardName} to {email}");
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        /// <summary>
        /// This method removes a board to the specific user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board to remove</param>
        /// <returns>The string "{}", unless an error occurs, then returns a string contains the error occured</returns>
        public string RemoveBoard(string email, string boardName)
        {
            try
            {
                if (userController.IsLoggedIn(email))
                {
                    boardController.RemoveBoard(email, boardName);
                    log.Info($"removed board: {boardName} to {email}");
                    return JsonSerializer.Serialize(new Response());
                }
                else
                {
                    log.Warn($"{email} is not logged In when trying to Remove {boardName}");
                    return JsonSerializer.Serialize(new Response("Error: user isn't logged in"));
                }
            }
            catch (Exception ex)
            {
                log.Warn($"thrown exception: {ex.Message}, while trying to Remove {boardName} of {email}");
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        /// <summary>
        /// This method limits the number of tasks can be in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnName">The column name. </param>
        /// <param name="limit">The new limit value. A value of -1 means that there is no limit.</param>
        /// <returns>The string "{}",unless an error occurs, then returns a string contains the error occured</returns>
        public string LimitColumn(string email, string boardName, string columnName, int limit)
        {
            try
            {
                if (userController.IsLoggedIn(email))
                {
                    boardController.GetBoard(email, boardName).GetColumn(columnName).SetLimit(limit);
                    log.Info($"limited {columnName} with limit: {limit} in board: {boardName} to {email}");
                    return JsonSerializer.Serialize(new Response());
                }
                else
                {
                    log.Warn($"{email} is not logged In when trying to Limit {columnName} in {boardName}");
                    return JsonSerializer.Serialize(new Response("Error: user isn't logged in"));
                }
            }
            catch (Exception ex)
            {
                log.Warn($"thrown exception: {ex.Message}, while trying to limit {columnName} to limit:{limit} in board:{boardName} of {email}");
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column name.</param>
        /// <returns>Response with column limit value, unless an error occurs, then returns a string contains the error occured</returns>
        public string GetColumnLimit(string email, string boardName, string columnName)
        {
            try
            {
                if (userController.IsLoggedIn(email))
                {
                    int limit = boardController.GetBoard(email, boardName).GetColumn(columnName).GetColumnLimit();
                    log.Info($"limit of {columnName} is {limit} in board: {boardName} of {email}");
                    return JsonSerializer.Serialize(new Response(null, limit));
                }
                else
                {
                    log.Warn($"{email} is not logged In when trying to get {columnName} limit in {boardName}");
                    return JsonSerializer.Serialize(new Response("Error: user isn't logged in"));
                }
            }
            catch (Exception ex)
            {
                log.Warn($"thrown exception: {ex.Message}, while trying to get the limit of {columnName} in board:{boardName} of {email}");
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>Response with  a list of the column's tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumnTasks(string email, string boardName, string columnName)
        {
            try
            {
                if (userController.IsLoggedIn(email))
                {
                    List<BusinessLayer.Task> tasks = boardController.GetBoard(email, boardName).GetColumnTasks(columnName);
                    List<TaskToSend> tasksToSend = new List<TaskToSend>();
                    foreach (BusinessLayer.Task task in tasks)
                    {
                        tasksToSend.Add(new TaskToSend(task.id, task.CreationTime, task.Title, task.Description, task.DueDate));
                    }
                    log.Info($"returned task list of {columnName} of board {boardName} of {email}");
                    var options = new JsonSerializerOptions()
                    {
                        IncludeFields = true,
                    };
                    return JsonSerializer.Serialize(new Response(null, tasksToSend), options);
                }
                else
                {
                    log.Warn($"{email} is not logged In when trying to get {columnName} tasks in {boardName}");
                    return JsonSerializer.Serialize(new Response("Error: user isn't logged in"));
                }
            }
            catch (Exception ex)
            {
                log.Warn($"thrown exception: {ex.Message}, while trying to get tasks of column:{columnName} in board:{boardName} of {email}");
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date of the new task</param>
        /// <returns>The string "{}", unless an error occurs, then returns a string contains the error occured</returns>
        public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                if (userController.IsLoggedIn(email))
                {
                    boardController.GetBoard(email, boardName).AddTask(title, dueDate, description, "backlog");
                    log.Info($"added new task to {boardName} of {email}");
                    return JsonSerializer.Serialize(new Response());
                }
                else
                {
                    log.Warn($"{email} is not logged In when trying to get add new task to {boardName}");
                    return JsonSerializer.Serialize(new Response("Error: user isn't logged in"));
                }
            }
            catch (Exception ex)
            {
                log.Warn($"thrown exception: {ex.Message}, while trying to add new task to board:{boardName} of {email}");
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        /// <summary>
        /// This method moves a task to the next column.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="currentColumn">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task Id of the task that needs to be to be updated</param>
        /// <returns>The string "{}", unless an error occurs, then returns a string contains the error occured</returns>
        public string MoveTask(string email, string boardName, string currentColumn, int taskId)
        {
            try
            {
                if (userController.IsLoggedIn(email))
                {
                    if (currentColumn.Equals("backlog"))
                    {
                        boardController.GetBoard(email, boardName).MoveTask(email, currentColumn, "in progress", taskId);
                        log.Info($"moved task number {taskId} in board {boardName} from column: {currentColumn} to column: in progress");
                        return JsonSerializer.Serialize(new Response());
                    }
                    else if (currentColumn.Equals("in progress"))
                    {
                        boardController.GetBoard(email, boardName).MoveTask(email, currentColumn, "done", taskId);
                        log.Info($"moved task number {taskId} in board {boardName} from column: {currentColumn} to column: done");
                        return JsonSerializer.Serialize(new Response());
                    }
                    else
                    {
                        log.Warn($"can't move task: {taskId} from {currentColumn} in {boardName} of {email}");
                        return JsonSerializer.Serialize(new Response("Error: can't move the task"));
                    }
                }
                else
                {
                    log.Warn($"{email} is not logged In when trying to move task {taskId} from {currentColumn} in {boardName}");
                    return JsonSerializer.Serialize(new Response("Error: user isn't logged in"));
                }
            }
            catch (Exception ex)
            {
                log.Warn($"thrown exception: {ex.Message}, while trying to move task: {taskId} from: {currentColumn} in board:{boardName} of {email}");
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        /// <summary>
        /// This method returns all the "In progress" tasks of the user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>Response with a list of the in progress tasks, unless an error occurs, <returns>A string contains the error occured</returns>
        public string GetTasksInProgress(string email)
        {
            try
            {
                if (userController.IsLoggedIn(email))
                {
                    List<BusinessLayer.Task> tasks = boardController.GetTasksInProgress(email);
                    List<TaskToSend> tasksToSend = new List<TaskToSend>();
                    foreach (BusinessLayer.Task task in tasks)
                    {
                        tasksToSend.Add(new TaskToSend(task.id, task.CreationTime, task.Title, task.Description, task.DueDate));
                    }
                    var options = new JsonSerializerOptions()
                    {
                        IncludeFields = true,
                    };
                    log.Info($"returned task list in progress of {email}");
                    return JsonSerializer.Serialize(new Response(null, tasksToSend), options);
                }
                else
                {
                    log.Warn($"{email} is not logged In when trying to get his tasks in progress");
                    return JsonSerializer.Serialize(new Response("Error: user isn't logged in"));
                }
            }
            catch (Exception ex)
            {
                log.Warn($"thrown exception: {ex.Message}, while trying to get tasks in progress of {email}");
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            try
            {
                if (userController.IsLoggedIn(currentOwnerEmail))
                {
                    boardController.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
                    log.Info($"Successfully Transfered Ownership of {boardName} from {currentOwnerEmail} to {newOwnerEmail}");
                    return JsonSerializer.Serialize(new Response());
                }
                else
                {
                    log.Warn($"{currentOwnerEmail} is not logged In when trying to TransferOwnership of {boardName} from {currentOwnerEmail} to {newOwnerEmail}");
                    return JsonSerializer.Serialize(new Response("Error: user isn't logged in"));
                }
            }
            catch (Exception ex)
            {
                log.Warn($"thrown exception: {ex.Message}, while trying to TransferOwnership of {boardName} from {currentOwnerEmail} to {newOwnerEmail}");
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>

        public string JoinBoard(string email, int boardID)
        {
            try
            {
                if (userController.IsLoggedIn(email))
                {
                    boardController.JoinBoard(email, boardID);
                    log.Info($"{email} joined board {boardID}");
                    return JsonSerializer.Serialize(new Response());
                }
                else
                {
                    log.Warn($"{email} is not logged In when trying to Join board {boardID}");
                    return JsonSerializer.Serialize(new Response("Error: user isn't logged in"));
                }
            }
            catch (Exception ex)
            {
                log.Warn($"thrown exception: {ex.Message}, while trying to join {email} to {boardID}");
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        /// <summary>
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LeaveBoard(string email, int boardID)
        {
            try
            {
                if (userController.IsLoggedIn(email))
                {
                    boardController.LeaveBoard(email, boardID);
                    log.Info($"{email} left board {boardID}");
                    return JsonSerializer.Serialize(new Response());
                }
                else
                {
                    log.Warn($"{email} is not logged In when trying to leave board {boardID}");
                    return JsonSerializer.Serialize(new Response("Error: user isn't logged in"));
                }
            }
            catch (Exception ex)
            {
                log.Warn($"thrown exception: {ex.Message}, while {email} trying to leave {boardID}");
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        public string LoadData()
        {
            try
            {
                boardController.LoadData();
                userController.LoadData();
                log.Info($"Loaded All data to BoardController");
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                log.Fatal($"thrown exception: {ex.Message}, while trying to load data from BoardController");
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        public string DeleteAllData()
        {
            try
            {
                boardController.DeleteAllData();
                userController.DeleteAllData();
                log.Info($"Deleted All data from BoardController");
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                log.Fatal($"thrown exception: {ex.Message}, while trying to delete data from BoardController");
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        public string GetUserBoards(string email)
        {
            try
            {
                if (userController.UserExists(email))
                {
                    List<int> boards = boardController.GetUserBoards(email);

                    log.Info($"returned list with the board IDs of {email}");
                    return JsonSerializer.Serialize(new Response(null, boards));
                }
                else
                {
                    log.Warn($"{email} is not registered In when trying to get his boards");
                    return JsonSerializer.Serialize(new Response("Error: user isn't registered"));
                }
            }
            catch (Exception ex)
            {
                log.Warn($"thrown exception: {ex.Message}, while trying to get all the boards of {email}");
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        public string GetBoardName(int boardId)
        {
            try
            {

                string boardName = boardController.GetBoardName(boardId);

                log.Info($"returned the name of board number {boardId}");
                return JsonSerializer.Serialize(new Response(null, boardName));

            }
            catch (Exception ex)
            {
                log.Warn($"thrown exception: {ex.Message}, while trying to get the name of board number {boardId}");
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }
    }
}
