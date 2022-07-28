using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.Model
{
    public class TaskModel : NotifiableModelObject
    {
        public int id;
        public int Id
        {
            get => id;
            set
            {
                this.id = value;
                RaisePropertyChanged("Id");
            }
        }
        public DateTime creationTime;

        public DateTime CreationTime
        {
            get => creationTime;
            set
            {
                this.creationTime = value;
                RaisePropertyChanged("CreationTime");
            }
        }
        public string title;
        public string Title
        {
            get => title;
            set
            {
                this.title = value;
                RaisePropertyChanged("Title");
            }
        }
        public string description;
        public string Description
        {
            get => description;
            set
            {
                this.description= value;
                RaisePropertyChanged("Description");
            }
        }
        public DateTime dueDate;
        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                this.dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }

        public TaskModel(BackendController controller, TaskToSend taskToSend) : base(controller)
        {
            this.Id = taskToSend.Id;
            this.CreationTime = taskToSend.CreationTime;
            this.Description = taskToSend.Description;
            this.DueDate = taskToSend.DueDate;
            this.Title = taskToSend.Title;
        }
    }
}
