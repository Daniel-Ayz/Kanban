using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    public class UserViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }

        public UserViewModel()
        {
            this.Controller = new BackendController();
            this.Username = "mail@mail.com";
            this.Password = "Password1";
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                this._username = value;
                RaisePropertyChanged("Username");
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                this._password = value;
                RaisePropertyChanged("Password");
            }
        }

        private string message;
        public string Message
        {
            get => message;
            set
            {
                this.message = value;
                RaisePropertyChanged("Message");
            }
        }

        public UserModel Login()
        {
            Message = "";
            try
            {
                return Controller.Login(Username, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }
        public UserModel Register()
        {
            Message = "";
            try
            {
                Message = "Registered successfully";
                return Controller.Register(Username, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }

    }
}
