using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ServiceFactory
    {
        public UserService UserService;
        public TaskService TaskService;
        public BoardService BoardService;
        private BusinessLayer.UserController userController;
        private BusinessLayer.BoardController boardController;
        
        public ServiceFactory()
        {
            userController = new BusinessLayer.UserController();
            boardController = new BusinessLayer.BoardController();
            UserService = new UserService(userController);
            TaskService = new TaskService(userController, boardController);
            BoardService = new BoardService(boardController, userController);

        }
    }
}
