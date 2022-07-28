using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskService
    {
        private BusinessLayer.BoardController boardcontroller;
        private BusinessLayer.UserController userController;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        internal TaskService(BusinessLayer.UserController userController, BusinessLayer.BoardController boardController) 
        {
            this.boardcontroller = boardController;
            this.userController = userController;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        /// <summary>
        /// This method updates the due date of a task.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="currentColumn">The current column.</param>
        /// <param name="taskId">The task Id of the task that needs to be to be updated</param>
        /// <param name="dueDate">The new due date of the task</param>
        /// <returns>The string "{}", unless an error occurs, <returns>A string contains the error occured</returns>
        public string UpdateTaskDueDate(string email, string boardName, string currentColumn, int taskId, DateTime dueDate)
        {
            try
            {
                if (currentColumn.Equals("done"))
                {
                    log.Warn($"can not edit tasks in column that is: {currentColumn}");
                    return JsonSerializer.Serialize(new Response("can not edit task that is done"));
                }
                else if (!userController.IsLoggedIn(email))
                {
                    log.Warn($"{email} is not logged In when trying to change due date to {dueDate}");
                    return JsonSerializer.Serialize(new Response("email not exist"));
                }
                else
                {
                    boardcontroller.GetBoard(email, boardName).GetColumn(currentColumn).GetTask(taskId).UpdateTaskDueDate(email,dueDate);
                    log.Info($"due date changed to: {dueDate}");
                    return JsonSerializer.Serialize(new Response());
                }
            }
            catch (Exception ex)
            {
                log.Warn($"thrown exception: {ex.Message}, while trying to change due date to {dueDate}");
                return JsonSerializer.Serialize(new Response(ex.Message));
            }  
        }
        /// <summary>
        /// This method updates the title of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="currentColumn">The current column.</param>
        /// <param name="taskId">The task Id of the task that needs to be to be updated</param>
        /// <param name="title">New title for the task</param>
        /// <returns>The string "{}", unless an error occurs, <returns>A string contains the error occured</returns>
        public string UpdateTaskTitle(string email, string boardName, string currentColumn, int taskId, string title)
        {
            try
            {
                if (currentColumn.Equals("done"))
                {
                    log.Warn($"can not edit tasks in column that is: {currentColumn}");
                    return JsonSerializer.Serialize(new Response("can not edit task that is done"));
                }
                else if (!userController.IsLoggedIn(email))
                {
                    log.Warn($"{email} is not logged In when trying to change title to {title}");
                    return JsonSerializer.Serialize(new Response("email not exist"));
                }
                else
                {
                    boardcontroller.GetBoard(email, boardName).GetColumn(currentColumn).GetTask(taskId).UpdateTaskTitle(email,title);
                    log.Info($"title changed to: {title}");
                    return JsonSerializer.Serialize(new Response());
                }
            }
            catch (Exception ex)
            {
                log.Warn($"thrown exception: {ex.Message}, while trying to change title to {title}");
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="currentColumn">The current column.</param>
        /// <param name="taskId">The task Id of the task that needs to be to be updated</param>
        /// <param name="description">New description for the task</param>
        /// <returns>The string "{}", unless an error occurs, <returns>A string contains the error occured</returns>
        public string UpdateTaskDescription(string email, string boardName, string currentColumn, int taskId, string description)
        {
            try
            {
                if (currentColumn.Equals("done"))
                {
                    log.Warn($"can not edit tasks in column that is: {currentColumn}");
                    return JsonSerializer.Serialize(new Response("can not edit task that is done"));
                }
                else if (!userController.IsLoggedIn(email))
                {
                    log.Warn($"{email} is not logged In when trying to change description to {description}");
                    return JsonSerializer.Serialize(new Response("email not exist"));
                }
                else
                {
                    boardcontroller.GetBoard(email, boardName).GetColumn(currentColumn).GetTask(taskId).UpdateTaskDescription(email,description);
                    log.Info($"description changed to: {description}");
                    return JsonSerializer.Serialize(new Response());
                }
            }
            catch (Exception ex)
            {
                log.Warn($"thrown exception: {ex.Message}, while trying to change description to {description}");
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="currentColumn">The current column.</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>The string "{}", unless an error occurs, <returns>A string contains the error occured</returns>
        public string AssignTask(string email, string boardName, string currentColumn, int taskID, string emailAssignee) 
        {
            try
            {
                if (!boardcontroller.IsMemberOfBoard(emailAssignee, boardName))
                {
                    log.Warn($"can not assign task to a user that is not a member of the board: {emailAssignee}");
                    return JsonSerializer.Serialize(new Response("can not assign task to a user that is not a member of the board "));
                }

                boardcontroller.GetBoard(email, boardName).GetColumn(currentColumn).GetTask(taskID).AssignTask(email, emailAssignee);
                log.Info($"task assigned to to: {emailAssignee}");
                return JsonSerializer.Serialize(new Response());
             
            }
            catch (Exception ex)
            {
                log.Warn($"thrown exception: {ex.Message}, while trying to assign task to {emailAssignee}");
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }
    }
}
