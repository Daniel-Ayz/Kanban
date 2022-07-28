using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Model;

namespace Frontend.ViewModel
{
    public class BoardTasksViewModel : NotifiableObject
    {
        private BackendController controller;
        private BoardModel board;
        private UserModel user;

        public BoardTasksModel BoardTasks { get; private set; }
        public string Title { get; private set; }

        public BoardTasksViewModel(UserModel user, BoardModel board)
        {
            this.controller = board.Controller;
            this.board = board;
            this.user = user;
            Title = "Board Id: " + board.Id + " | Board Name: " + board.Name;
            BoardTasks = board.GetBoardTasks(user);
        }
    }
}
