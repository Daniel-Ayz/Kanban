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
    internal class Board
    {
        internal readonly int id;
        internal readonly string name;
        private Dictionary<string, Column> columns;
        private int idSerializer;
        private string owner;
        private BoardDTO boardDTO;

        private Board(string name, int id, string email)
        {
            this.id = id;
            this.name = name;
            this.columns = new Dictionary<string, Column>();
            this.idSerializer = 0;
            this.owner = email;
        }

        internal Board(BoardDTO boardDTO)
        {
            this.id=boardDTO.Id;
            this.name = boardDTO.Name;
            this.columns = new Dictionary<string, Column>(); 
            this.idSerializer = 0; 
            this.owner = boardDTO.OwnerEmail;
            this.boardDTO=boardDTO;
        }


        internal static Board BuildStandardBoard(string boardName, int id, string owner)
        {
            BoardDTO boardDTO = new BoardDTO(id, boardName, owner);
            boardDTO.Persist();
            Board board = new Board(boardName,id,owner);
            board.boardDTO = boardDTO;
            board.AddColumn("backlog");
            board.AddColumn("in progress");
            board.AddColumn("done");
            return board;
        }

        internal string Owner{ get { return owner; }}
        internal int IdSerializer { get => idSerializer;  set { idSerializer = value; } }

        private bool columnExists(string columnName)
        {
            return columns.ContainsKey(columnName);
        }

        internal Column GetColumn(string columnName)
        {
            if (columnExists(columnName))
                return columns[columnName];
            else
                throw new Exception("column doesn't exist");
        }

        internal List<Task> GetColumnTasks(string columnName)
        {
            if (!columnExists(columnName))
                throw new Exception("there is no such column name");
            List<Task> tasksInColumn = columns[columnName].GetTasks().Values.ToList();
            return tasksInColumn;
        }

        internal void AddColumn(string columnName)
        {
            if (columnExists(columnName))
                throw new ArgumentException("column already exist");
            columns.Add(columnName, Column.BuildColumn(columnName, id));
        }

        internal void RetriveColumn(Column column)
        {
            columns.Add(column.name, column);
        }

        internal void AddTask(string title, DateTime dueDate, string description, string columnName)
        {
            if (!columnExists(columnName))
                throw new ArgumentException("Column doesn't exist");
            Column c = columns[columnName];
            c.AddTask(title, dueDate, description, idSerializer, this.id);
            idSerializer++;


        }

        internal void MoveTask(string email, string currentColumn, string nextColumn, int taskId)
        {
            if (!columnExists(currentColumn) || !columnExists(nextColumn))
                throw new ArgumentException("Column doesn't exist");
            if (GetColumn(currentColumn).GetTask(taskId).AssigneeEmail != email)
                throw new Exception("only the assignee can move tasks");
            Column c1 = columns[currentColumn];
            Column c2 = columns[nextColumn];
            if (c2.IsFull())
                throw new Exception("target column is full");
            if (!c1.ContainsTask(taskId))
                throw new Exception("there is no such task");
            Task task = c1.DeleteTask(taskId);
            task.TaskDTO.ColumnName = nextColumn;
            c2.AddTask(task);
        }

        internal void TransferOwnership(string currentOwnerEmail, string newOwnerEmail)
        {
            if (!owner.Equals(currentOwnerEmail))
                throw new Exception($"the {currentOwnerEmail} isn't the real owner");
            else
            {
                owner = newOwnerEmail;
                boardDTO.OwnerEmail = owner;

            }
        }
    }
}