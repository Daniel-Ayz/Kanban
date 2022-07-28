using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DAL;

[assembly: InternalsVisibleTo("BusinessTests")]
namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class Column
    {
        private const int NO_LIMIT = -1;

        internal readonly string name;
        private int maxTasks;
        private Dictionary<int, Task> tasksList;
        private ColumnDTO columnDTO;


        private Column(string name, int maxTasks)
        {
            this.name = name;
            this.maxTasks = maxTasks;
            this.tasksList = new Dictionary<int, Task>();
        }

        internal static Column BuildColumn(string name, int boardID, int limit = NO_LIMIT)
        {
            ColumnDTO columnDTO = new ColumnDTO(boardID, name, limit);
            columnDTO.Persist();
            Column column = new Column(name, limit);
            column.columnDTO = columnDTO;
            return column;
        }

        internal Column(ColumnDTO columnDTO)
        {
            this.name = columnDTO.ColumnName;
            this.maxTasks = columnDTO.MaxTasks;
            tasksList = new Dictionary<int, Task>();
            this.columnDTO = columnDTO;
        }

        internal Dictionary<int, Task> GetTasks()
        {
            return tasksList;
        }

        internal Task GetTask(int taskId)
        {
            if (!ContainsTask(taskId))
                throw new Exception("column does not have this task");
            return tasksList[taskId];
        }

        internal int GetColumnLimit()
        {
            return maxTasks;
        }

        internal void SetLimit(int limit)
        {
            if (limit == NO_LIMIT)
            {
                maxTasks = NO_LIMIT;
                columnDTO.MaxTasks = maxTasks;
            }
            else if (limit < tasksList.Count)
                throw new Exception("there are more tasks at the column currently, than the wanted limit");
            else
            {
                maxTasks = limit;
                columnDTO.MaxTasks = maxTasks;
            }
        }

        internal bool IsFull()
        {
            if (maxTasks == NO_LIMIT)
                return false;
            else
                return tasksList.Count == maxTasks;
        }

        internal bool ContainsTask(int taskId)
        {
            return tasksList.ContainsKey(taskId);
        }

        internal void AddTask(string title, DateTime dueDate, string description, int idSerializer, int boardID)
        {
            if (!Task.ValidateTask(title,dueDate,description))
                throw new Exception("invalid task parameters");
            if (IsFull())
                throw new Exception("Column reached maximum number of tasks");
            Task task = Task.BulidTask(dueDate, title, description, idSerializer, boardID, this.name);
            tasksList.Add(task.id, task);

        }

        internal void AddTask(Task task)
        {
            if (task == null)
                throw new ArgumentNullException("task cannot be null");
            if (IsFull())
                throw new Exception("Column reached maximum number of tasks");
            tasksList.Add(task.id, task);
        }

        internal void RetriveTask(Task task)
        {
            tasksList.Add(task.id, task);
        }

        internal Task DeleteTask(int taskId)
        {
            if (!ContainsTask(taskId))
                throw new Exception("no such task in this column");
            Task task = tasksList[taskId];
            tasksList.Remove(taskId);
            return task;
        }
    }
}