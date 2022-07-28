using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DAL
{
    internal class LoadMapper
    {
        TaskMapper taskMapper;
        ColumnMapper columnMapper;
        UserToBoardMapper userToBoardMapper;

        internal LoadMapper()
        {
            taskMapper = new TaskMapper();
            columnMapper = new ColumnMapper();
            userToBoardMapper = new UserToBoardMapper();
        }

        internal List<TaskDTO> SelectAllTasks()
        {
            return taskMapper.SelectAllTasks();
        }

        internal List<ColumnDTO> SelectAllColumn()
        {
            return columnMapper.SelectAllColumns();
        }

        internal List<UserToBoardDTO> SelectAllUserToBoard()
        {
            return userToBoardMapper.SelectAllUserToBoard();
        }

        internal void DeleteAllData()
        {
            taskMapper.DeleteAllData();
            columnMapper.DeleteAllData();
            userToBoardMapper.DeleteAllData();
        }

    }
}
