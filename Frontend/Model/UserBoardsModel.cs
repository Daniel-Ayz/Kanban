using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class UserBoardsModel : NotifiableModelObject
    {
        private readonly UserModel user;
        public ObservableCollection<BoardModel> Boards { get; set; }

        public UserBoardsModel(BackendController controller, UserModel user) : base(controller)
        {
            this.user = user;
            Boards = new ObservableCollection<BoardModel>(controller.GetUserBoards(user.Email).
                Select((c, i) => new BoardModel(controller, c, controller.GetBoardName(c))).ToList());
        }
    }
}
