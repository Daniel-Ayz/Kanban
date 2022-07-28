using System;

namespace IntroSE.Kanban.Backend.DAL
{
    internal class TaskDTO : DTO
    {
        private int taskID;
        private int boardID;
        private string columnName;
        private string description;
        private string assigneeEmail;
        private string title;
        private DateTime creationTime;
        private DateTime dueDate;


        internal const string taskIDcolumnName = "taskID";
        internal const string boardIDcolumnName = "boardID";
        internal const string columnNamecolumnName = "columnName";
        internal const string descriptioncolumnName = "description";
        internal const string assigneeEmailcolumnName = "assigneeEmail";
        internal const string titlecolumnName = "title";
        internal const string creationTimecolumnName = "creationTime";
        internal const string dueDatecolumnName = "dueDate";



        internal DateTime CreationTime{ get { return creationTime; }}
        internal DateTime DueDate { get => dueDate; set { dueDate = value; ((TaskMapper)mapper).Update(taskID, boardID, dueDatecolumnName, dueDate); } }
        internal string Title { get => title; set { title = value; ((TaskMapper)mapper).Update(taskID, boardID, titlecolumnName, title); } }
        internal string Description { get => description; set { description = value; ((TaskMapper)mapper).Update(taskID, boardID, descriptioncolumnName, description); } }
        internal string AssigneeEmail { get => assigneeEmail; set { assigneeEmail = value; ((TaskMapper)mapper).Update(taskID, boardID, assigneeEmailcolumnName, assigneeEmail); } }
        internal int TaskId { get { return taskID; }}
        internal int BoardID{ get { return boardID; } }
        internal string ColumnName { get => columnName; set { columnName = value; ((TaskMapper)mapper).Update(taskID, boardID, columnNamecolumnName, ColumnName); } }

        internal TaskDTO( int taskID, int boardId, string columnName, string description, string assigneeEmail, string title, DateTime creationTime, DateTime dueDate) : base(new TaskMapper())
        {
            this.taskID = taskID;
            this.boardID = boardId;
            this.columnName = columnName;
            this.description = description;
            this.assigneeEmail = assigneeEmail;
            this.title = title;
            this.creationTime = creationTime;

            if (DateTime.Compare(creationTime, dueDate) < 0)
                this.dueDate = dueDate;

        }

        internal void Persist()
        {
            ((TaskMapper)mapper).Insert(this);
        }

    }
}
