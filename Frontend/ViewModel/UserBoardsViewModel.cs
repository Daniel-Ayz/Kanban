using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Frontend.ViewModel
{
    internal class UserBoardsViewModel : NotifiableObject
    {
        private BackendController controller;
        private UserModel user;

        public UserBoardsModel UserBoards { get; private set; }
        public string Title { get; private set; }
        private BoardModel selectedBoard;

        public UserBoardsViewModel(UserModel user)
        {
            this.controller = user.Controller;
            this.user = user;
            Title = "Boards of " + user.Email;
            UserBoards = user.GetUserBoards();
        }
        public BoardModel SelectedBoard
        {
            get
            {
                return selectedBoard;
            }
            set
            {
                selectedBoard = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedBoard");
            }
        }
        private bool _enableForward = false;
        public bool EnableForward
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }

        public BoardModel SelectBoard()
        {
            return selectedBoard;
        }
    }
}
