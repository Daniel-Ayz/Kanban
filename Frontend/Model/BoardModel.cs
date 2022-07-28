using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class BoardModel : NotifiableModelObject
    {
        private int id;
        public int Id
        {
            get => id;
            set
            {
                this.id = value;
                RaisePropertyChanged("Id");
            }
        }
        private string name;
        public string Name
        {
            get => name;
            set
            {
                this.name = value;
                RaisePropertyChanged("Name");
            }
        }

        public BoardModel(BackendController controller, int id, string name) : base(controller)
        {
            this.Id = id;
            this.Name = name;
        }

        public BoardTasksModel GetBoardTasks(UserModel user)
        {
            return new BoardTasksModel(Controller, user, this);
        }
    }
}
