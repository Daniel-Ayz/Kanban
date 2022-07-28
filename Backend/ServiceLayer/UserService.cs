using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;

namespace IntroSE.Kanban.Backend.ServiceLayer
{

    public class UserService
    {
        private BusinessLayer.UserController userController;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        internal UserService(BusinessLayer.UserController userController) 
        { 
            this.userController = userController;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }


        /// <summary>
        ///  This method logs in to an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>Response with user email, unless an error occurs, <returns>A string contains the error occured</returns>
        public string Login(string email, string password)
        {
            try
            {
                if (userController.UserExists(email))
                {
                    userController.GetUser(email).Login(password);
                    log.Info($"logged in the user: {email}");
                    return JsonSerializer.Serialize(new Response(null, email));
                }
                else
                {
                    log.Warn($"user {email} does not exist");
                    return JsonSerializer.Serialize(new Response("Error: the user does not exist"));
                }
                    
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out. Must be logged in</param>
        /// <returns>The string "{}",  unless an error occurs, A string contains the error occured</returns>
        public string Logout(string email)
        {
            try
            {
                if (userController.UserExists(email))
                {
                    userController.GetUser(email).Logout();
                    log.Info($"The user: {email} logged out");
                    return JsonSerializer.Serialize(new Response());
                }
                else
                {
                    log.Warn($"user {email} does not exist");
                    return JsonSerializer.Serialize(new Response("Error: the user does not exist"));
                }
                    
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }

        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging to the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>The string "{}",  unless an error occurs, <returns>A string contains the error occured</returns>
        public string Register(string email, string password)
        {
            try
            {
                if (!userController.UserExists(email))
                {
                    userController.Register(email, password);
                    log.Info($"The user: {email} is registered");
                    return JsonSerializer.Serialize(new Response());
                }
                else
                {
                    log.Warn($"user {email} is already exists");
                    return JsonSerializer.Serialize(new Response("Error: the user is already exists"));
                }
                    
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new Response(ex.Message));
            }
        }
 
    }

}
