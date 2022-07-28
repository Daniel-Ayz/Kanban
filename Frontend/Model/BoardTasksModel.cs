using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class BoardTasksModel : NotifiableModelObject
    {
        private readonly UserModel user;
        private readonly BoardModel board;
        public ObservableCollection<TaskModel> BacklogTasks { get; set; }
        public ObservableCollection<TaskModel> InProgressTasks { get; set; }
        public ObservableCollection<TaskModel> DoneTasks { get; set; }

        public BoardTasksModel(BackendController controller,UserModel User, BoardModel Board) : base(controller)
        {
            this.board = Board;
            this.user = User;
            BacklogTasks = new ObservableCollection<TaskModel>(controller.GetColumnTasks(user.Email, board.Name, "backlog"));
            InProgressTasks = new ObservableCollection<TaskModel>(controller.GetColumnTasks(user.Email, board.Name, "in progress"));
            DoneTasks = new ObservableCollection<TaskModel>(controller.GetColumnTasks(user.Email, board.Name, "done"));
        }
    }
}
