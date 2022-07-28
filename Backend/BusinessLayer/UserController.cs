using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DAL;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class UserController
    {
        private Dictionary<string, User> users;
        private UserMapper userMapper;


        internal UserController()
        {
            users = new Dictionary<string, User>();
            userMapper = new UserMapper();
        }

        ///<summary>This method loads all persisted data.
        ///<para>
        /// </summary>
        /// <returns> none, unless an error occurs,throws exception with appropriate message</returns>
        internal void LoadData()
        {
            //reset RAM
            users = new Dictionary<string, User>();
            //Load all users
            List<UserDTO> usersDTO = userMapper.SelectAllUsers();
            foreach (UserDTO userDTO in usersDTO)
            {
                User user = new User(userDTO);
                users.Add(user.email, user);

            }
        }

        ///<summary>This method deletes all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> 
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        /// <returns> none, unless an error occurs,throws exception with appropriate message</returns>
        internal void DeleteAllData()
        {
            userMapper.DeleteAllData();
            users = new Dictionary<string, User>();
        }

        internal bool UserExists(string email) {
            return users.ContainsKey(email);
        }


        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns> none, unless an error occurs,throws exception with appropriate message</returns>
        internal void Register(string email, string password) {
            if (email == null)
                throw new ArgumentNullException("Error: Email is null");
            if (UserExists(email))
                throw new Exception("Error: The email is already in use");
            if (!User.ValidatePassword(password))
                throw new Exception("Error- Password is not valid");
            if (!User.ValidateEmail(email))
                throw new Exception("Error- Email is not valid");
          
            User toCreate = User.BuildUser(email, password);
            users.Add(email, toCreate);

            UserDTO dto = new UserDTO(email, password);
            
        }

        internal void DeleteUser(string email) {
            if (!UserExists(email))
                throw new ArgumentException("Error- User is not exist");
            users.Remove(email);
        }

        internal bool IsLoggedIn(string email) {
            if (!UserExists(email))
                throw new ArgumentException("Error- User is not exist");
            
            return users[email].LoggedIn;
        }

        internal User GetUser(string email) {
            if (!UserExists(email))
                throw new ArgumentException("Error- User is not exist");
            return users[email];
        }

    }
}
