using System;

namespace IntroSE.Kanban.Backend.DAL
{
    internal class ColumnDTO : DTO
    {
        private int boardID;
        private string columnName;
        private int maxTasks;

        internal const string boardIDcolumnName = "boardID";
        internal const string columnNamecolumnName = "columnName";
        internal const string maxTaskscolumnName = "maxTasks";

        internal string ColumnName { get { return columnName; } }
        internal int BoardID { get { return boardID; } }

        internal int MaxTasks { get => maxTasks; set { maxTasks = value; ((ColumnMapper)mapper).Update(boardID, columnName, maxTaskscolumnName, maxTasks); } }

        internal ColumnDTO(int boardId, string columnName, int maxTasks) : base(new ColumnMapper())
        {
            this.boardID = boardId;
            this.columnName = columnName;
            this.maxTasks = maxTasks;
        }

        internal void Persist()
        {
            ((ColumnMapper)mapper).Insert(this);
        }

    }
}
