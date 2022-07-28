using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DAL;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class Task
    {
        private const int MAX_TITLE_LEN = 50;
        private const int MAX_DESCRIPTION_LEN = 300;
        private const string UNASSIGNED_TASK = "unassigned";

        internal int id { get; }
        private readonly DateTime creationTime;
        private DateTime dueDate;
        private string title;
        private string description;
        private string assigneeEmail= UNASSIGNED_TASK;
        private TaskDTO taskDTO;

        private Task(DateTime dueDate, string title, string description, int id)
        {
            this.id = id;
            this.creationTime = DateTime.Now;
            this.dueDate = dueDate;
            this.title = title;
            this.description = description;  
        }
        
        internal static Task BulidTask(DateTime dueDate, string title, string description, int id, int boardID, string columnName)
        {
            TaskDTO taskDTO = new TaskDTO(id, boardID, columnName, description, UNASSIGNED_TASK , title, DateTime.Now, dueDate);
            taskDTO.Persist();
            Task task = new Task(dueDate, title, description, id);  
            task.taskDTO = taskDTO;
            return task;
        }

        internal Task(TaskDTO taskDTO)
        {
            this.id = taskDTO.TaskId;
            this.creationTime = taskDTO.CreationTime;
            this.dueDate = taskDTO.DueDate;
            this.title = taskDTO.Title;
            this.description=taskDTO.Description;
            this.assigneeEmail = taskDTO.AssigneeEmail;
            this.taskDTO = taskDTO;
        }

        internal static bool ValidateTask(string title, DateTime dueDate, string description)
        {
            if (DateTime.Compare(DateTime.Now, dueDate) >= 0)
                return false;
            if (title == null || title.Length==0 || title.Length > MAX_TITLE_LEN)
                return false;
            if (description == null || description.Length > MAX_DESCRIPTION_LEN)
                return false;
            return true;
        }
        
        public DateTime CreationTime
        {
            get { return creationTime; }
        }

        internal DateTime DueDate
        {
            get { return dueDate; }
        }

        internal string Title
        {
            get { return title; }
        }

        internal string Description
        {
            get { return description; }
        }
        internal string AssigneeEmail
        {
            get { return assigneeEmail; }
        }
        internal TaskDTO TaskDTO
        {
            get { return taskDTO; }
        }


        /// <summary>
        /// This method updates the due date of a task.
        /// </summary>
        /// <param name="newDueDate">The new due date of the task</param>
        /// <returns> none, unless an error occurs, <returns>throws exception with appropriate message</returns>
        internal void UpdateTaskDueDate(string email, DateTime newDueDate)
        {
            if (DateTime.Compare(newDueDate, CreationTime) > 0)
            {
                taskDTO.DueDate = newDueDate;
                dueDate = newDueDate;
            }
            else if(email!= assigneeEmail)
            {
                throw new Exception("Error: task that is not done can be changed by it's assignee only.");
            }
            else
            {
                throw new Exception("Error: new due date passed");
            }
        }

        /// <summary>
        /// This method updates the title of a task.
        /// </summary>
        /// <param name="newTitle">New title for the task</param>
        /// <returns> none, unless an error occurs, <returns>throws exception with appropriate message</returns>
        internal void UpdateTaskTitle(string email, string newTitle)
        {
            if (newTitle == null)
            {
                throw new Exception("Error: title cannot be null");
            }
            else if (email != assigneeEmail)
            {
                throw new Exception("Error: task that is not done can be changed by it's assignee only.");
            }
            else if (newTitle.Length == 0)
            {
                throw new Exception("Error: title cannot be empty");
            }
            else if (newTitle.Length > MAX_TITLE_LEN)
            {
                throw new Exception("Error: title cannot be larger than 50 characters");
            }
            else
            {
                taskDTO.Title = newTitle;
                this.title = newTitle;
            }
        }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="newDescription">New description for the task</param>
        /// <returns> none, unless an error occurs, <returns>throws exception with appropriate message</returns>
        internal void UpdateTaskDescription(string email, string newDescription)
        {
            if (newDescription == null)
            {
                throw new Exception("description cannot be null");
            }
            else if (email != assigneeEmail)
            {
                throw new Exception("Error: task that is not done can be changed by it's assignee only.");
            }
            else if (newDescription.Length > MAX_DESCRIPTION_LEN)
            {
                throw new Exception("description cannot be larger than 300 characters");
            }
            else
            {
                taskDTO.Description = newDescription;
                this.description = newDescription;
            }
        }

        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns> none, unless an error occurs, <returns>throws exception with appropriate message</returns>
        internal void AssignTask(string email,string emailAssignee)
        {
            if(assigneeEmail == UNASSIGNED_TASK || assigneeEmail == email)
            {
                taskDTO.AssigneeEmail = emailAssignee;
                assigneeEmail = emailAssignee;
               
            }
            else
            {
                throw new Exception("task is already assigned to a user that is not the current user ");
            }

        }

        /// <summary>
        /// This method unassigns a task.
        /// </summary>
        /// <returns> none</returns>
        internal void UnassignTask()
        {
            taskDTO.AssigneeEmail = UNASSIGNED_TASK;
            assigneeEmail =UNASSIGNED_TASK;
        }
    }
}