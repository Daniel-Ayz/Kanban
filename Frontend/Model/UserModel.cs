using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class UserModel : NotifiableModelObject
    {
        public string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        public UserModel(BackendController controller, string email) : base(controller)
        {
            this.Email = email;
        }

        public UserBoardsModel GetUserBoards()
        {
            return new UserBoardsModel(Controller, this);
        }
    }
}
