using System;
using System.Linq;
using System.Net.Mail;
using IntroSE.Kanban.Backend.DAL;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class User
    {
        private const int MIN_PASSWORD_LEN = 6;
        private const int MAX_PASSWORD_LEN = 20;

        internal readonly string email; 
        private string password;
        private bool loggedIn;
        private UserDTO userDTO;

        private User(string email, string password)
        {
            this.email = email;
            this.password = password;
            loggedIn = true;
        }

        internal static User BuildUser(string email, string password)
        {
            UserDTO userDTO = new UserDTO(email, password);
            userDTO.Persist();
            User user = new User(email, password);
            user.userDTO=userDTO;
            return user;
        }

        internal User(UserDTO userDTO)
        {
            email = userDTO.Email;
            password = userDTO.Password;
            loggedIn = false;
            this.userDTO = userDTO;
        }

        internal bool LoggedIn
        {
            get { return loggedIn; }
        }


        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns> none, unless an error occurs,throws exception with appropriate message</returns>
        internal void Login(string password)
        {
            if (password == null)
                throw new ArgumentNullException("Error: The password is null");

            if (!loggedIn)
            {
                if (!this.password.Equals(password))
                    throw new Exception("Error: The password is incorrect");

                loggedIn = true;
            }
        }

        
        internal static bool ValidatePassword(string password)
        {
            if (password == null || password.Length < MIN_PASSWORD_LEN || password.Length > MAX_PASSWORD_LEN)
                return false;
            
            if (!password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsNumber))
                return false;

            string regex = @"(^[a-zA-Z0-9!@#$%^&*()_+]{6,20}$)";
            bool valid = System.Text.RegularExpressions.Regex.IsMatch(password, regex);
            if (!valid)
                return false;

            return true;
            
        }

        internal static bool ValidateEmail(string email)
        {
            for( int i =0; i<email.Length; i++)
            {
                if (!Char.IsAscii(email[i]))
                {
                    return false;
                }
            }

            try
            {
                MailAddress m = new MailAddress(email);
                
                return true;
            }
            catch (FormatException)
            {
                return false;
            }

        }
      

        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns> none, unless an error occurs, throws exception with appropriate message</returns>
        internal void Logout()
        {
            if (loggedIn)
            {
                this.loggedIn = false;
            }
            else
                throw new Exception("Error: The user is not logged in");
        }
    }
}
