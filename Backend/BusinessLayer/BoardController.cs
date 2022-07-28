using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DAL;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class BoardController
    {
        private Dictionary<string, Dictionary<string, Board>> boards;
        private Dictionary<int, Board> boardsById;
        private int idSerializer;
        private BoardMapper boardMapper;
        private LoadMapper loadMapper;

        internal BoardController()
        {
            boards = new Dictionary<string, Dictionary<string, Board>>();
            boardsById = new Dictionary<int, Board>();
            idSerializer = 0;
            loadMapper = new LoadMapper();
            boardMapper = new BoardMapper();
        }

        internal void LoadData()
        {
            //reset RAM
            boards = new Dictionary<string, Dictionary<string, Board>>();
            boardsById = new Dictionary<int, Board>();
            //Load All boardsById
            List<BoardDTO> boardsDTORetrived = boardMapper.SelectAllBoards();
            int idCounter = -1;
            foreach (var boardDTO in boardsDTORetrived) 
            {
                Board board = new Board(boardDTO);
                boardsById.Add(board.id, board);
                idCounter = Math.Max(idCounter, board.id);
            }
            idSerializer = idCounter+1;
            //LoadAllUsersToBoards
            List<UserToBoardDTO> usersToBoardDTOs = loadMapper.SelectAllUserToBoard();
            foreach (var userToBoard in usersToBoardDTOs)
            {
                string email = userToBoard.Email;
                int boardId = userToBoard.BoardID;
                Board board = GetBoardByID(boardId);
                if (!boards.ContainsKey(email))
                {
                    Dictionary<string, Board> newBoards = new Dictionary<string, Board>();
                    boards.Add(email, newBoards);
                }
                boards[email].Add(board.name, board);
            }
            //LoadAllColumns
            List<ColumnDTO> columnsDTO = loadMapper.SelectAllColumn();
            foreach (var columnDTO in columnsDTO)
            {
                Column column = new Column(columnDTO);
                Board board = GetBoardByID(columnDTO.BoardID);
                board.RetriveColumn(column);
            }
            //LoadAllTasks
            List<TaskDTO> tasksDTO = loadMapper.SelectAllTasks();
            foreach (var taskDTO in tasksDTO)
            {
                Task task = new Task(taskDTO);
                Board board = GetBoardByID(taskDTO.BoardID);
                board.IdSerializer = Math.Max(board.IdSerializer , task.id + 1);
                Column column = board.GetColumn(taskDTO.ColumnName);
                column.RetriveTask(task);
            }
        }

        internal void DeleteAllData()
        {
            boardMapper.DeleteAllData();
            loadMapper.DeleteAllData();
            boards = new Dictionary<string, Dictionary<string, Board>>();
            boardsById = new Dictionary<int, Board>();
            idSerializer = 0;
        }

        internal Board GetBoardByID(int id)
        {
            if (boardsById.ContainsKey(id))
                return boardsById[id];
            else
                throw new Exception("there is no such board id");
        }

        internal Board GetBoard(string email, string boardName)
        {
            if (!IsMemberOfBoard(email, boardName))
                throw new ArgumentException("Error- The board is not exist");

            return boards[email][boardName];
        }

        internal bool IsMemberOfBoard(string email, string boardName) 
        {
            return boards.ContainsKey(email) && boards[email].ContainsKey(boardName);
        }

        internal void AddBoard(string email, string boardName)
        {
            if (email == null)
                throw new ArgumentNullException("Error- email is null");
            if (boardName == null)
                throw new ArgumentNullException("Error- board name is null");
            if (IsMemberOfBoard(email, boardName))
                throw new ArgumentException("Error- The user is already has a board with this name");

            Board toAdd = Board.BuildStandardBoard(boardName,idSerializer,email);
            if (!boards.ContainsKey(email))
            {
                Dictionary<string, Board> newBoards = new Dictionary<string, Board>();
                boards.Add(email, newBoards);
            }

            boards[email].Add(boardName, toAdd);   

            boardsById.Add(idSerializer, toAdd);

            BoardDTO dto = new BoardDTO(idSerializer, boardName, email);

            idSerializer++;   
            
        }

        internal void RemoveBoard(string email, string boardName)
        {
            if (email == null)
                throw new ArgumentNullException("Error: email is null");
            if (boardName == null)
                throw new ArgumentNullException("Error: board name is null");
            if (!IsMemberOfBoard(email, boardName))
                throw new ArgumentException("Error: The board is not exist");

            Board toRemove=boards[email][boardName];
            if (email != toRemove.Owner)
                throw new Exception("Error: The user is not the owner of the board");

            boards[email].Remove(boardName);

            boardsById.Remove(toRemove.id);

            if (boards[email].Count == 0)
                boards.Remove(email);

            BoardDTO dto = new BoardDTO(toRemove.id, toRemove.name, toRemove.Owner);
            boardMapper.Delete(dto);

        }

        internal List<Task> GetTasksInProgress(string email)
        {
            if (email == null)
                throw new ArgumentNullException("Error- email is null");

            List<Task> tasksInProgress = new List<Task>();
            if (!boards.ContainsKey(email))
                return tasksInProgress;

            foreach (var board in boards[email].Values)
            {
                Column inProgress = board.GetColumn("in progress");
                Dictionary<int, Task> tasks = inProgress.GetTasks();
                List<Task> taskList = new List<Task>();
                foreach (var task in tasks)
                {
                   if(task.Value.AssigneeEmail == email)
                    {
                        taskList.Add(task.Value);
                    }
                }
                tasksInProgress.AddRange(taskList);                
            }

            return tasksInProgress;
        }

        internal void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            if (IsMemberOfBoard(newOwnerEmail, boardName))
            {
                boards[currentOwnerEmail][boardName].TransferOwnership(currentOwnerEmail, newOwnerEmail);

            }
            else
                throw new Exception("the new owner isn't member of the board");
        }

        internal void JoinBoard(string email, int boardID)
        {
            if (email == null)
                throw new ArgumentNullException("Error: email is null");
            if (!boardsById.ContainsKey(boardID))
                throw new ArgumentNullException("Error: The board is not exist");
            if (IsMemberOfBoard(email, GetBoardByID(boardID).name))
                throw new ArgumentException("Error: The user is already has a board with this name");

            boardMapper.JoinBoard(email, boardID);

            Board toJoin = GetBoardByID(boardID);
            if (!boards.ContainsKey(email))
            {
                Dictionary<string, Board> newBoards = new Dictionary<string, Board>();
                boards.Add(email, newBoards);
            }

            boards[email].Add(toJoin.name, toJoin);
        }

        internal void LeaveBoard(string email, int boardID)
        {
            if (email == null)
                throw new ArgumentNullException("Error: email is null");
            if (!boardsById.ContainsKey(boardID))
                throw new ArgumentNullException("Error: The board is not exist");
            if (!IsMemberOfBoard(email, GetBoardByID(boardID).name))
                throw new ArgumentException("Error: The user is not signed to that board");

            boardMapper.LeaveBoard(email, boardID);

            Board toLeave = GetBoardByID(boardID);
            if (email == toLeave.Owner)
                throw new Exception("Error: The user is the owner of the board");

            List<Task> backlogTasks = toLeave.GetColumnTasks("backlog");
            List<Task> inProgressTasks = toLeave.GetColumnTasks("in progress");

            foreach (var task in backlogTasks)
            {
                if(task.AssigneeEmail == email)
                    task.UnassignTask();
            }

            foreach (var task in inProgressTasks)
            {
                if (task.AssigneeEmail == email)
                    task.UnassignTask();
            }

            boards[email].Remove(toLeave.name);
            if (boards[email].Count == 0)
                boards.Remove(email);
        }

        internal List<int> GetUserBoards(string email)
        {
            if (email == null)
                throw new ArgumentNullException("Error: email is null");
            
            List<int> boardIDs = new List<int>();
            if (boards.ContainsKey(email))
            {
                foreach (var board in boards[email])
                {
                    boardIDs.Add(board.Value.id);
                }
            }
            
            return boardIDs;
        }

        public string GetBoardName(int boardId)
        {
            if (!boardsById.ContainsKey(boardId))
                throw new ArgumentNullException("Error: The board is not exist");
            return boardsById[boardId].name;
        }
    }
}
