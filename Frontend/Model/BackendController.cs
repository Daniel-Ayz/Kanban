using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class BackendController
    {
        private ServiceFactory serviceFactory;
        public ServiceFactory ServiceFactory { get { return serviceFactory; } }
        public BackendController()
        {
            this.serviceFactory = new ServiceFactory();
            serviceFactory.BoardService.LoadData();
        }

        public UserModel Login(string username, string password)
        {
            string userJson = serviceFactory.UserService.Login(username, password);
            Response? response = JsonSerializer.Deserialize<Response>(userJson);
            if (response.ErrorMessage != null)
            {
                throw new Exception(response.ErrorMessage);
            }
            return new UserModel(this, username);
        }

        public UserModel Register(string username, string password)
        {
            string userJson = serviceFactory.UserService.Register(username, password);
            Response? response = JsonSerializer.Deserialize<Response>(userJson);
            if (response.ErrorMessage != null)
            {
                throw new Exception(response.ErrorMessage);
            }

            return new UserModel(this, username);
        }

        public List<int> GetUserBoards(string email)
        {
            string listJson = serviceFactory.BoardService.GetUserBoards(email);
            Response? response = JsonSerializer.Deserialize<Response>(listJson);
            if (response.ErrorMessage != null)
            {
                throw new Exception(response.ErrorMessage);
            }

            return JsonSerializer.Deserialize<List<int>>((JsonElement)(response.ReturnValue));
        }

        public string GetBoardName(int boardId)
        {
            string json = serviceFactory.BoardService.GetBoardName(boardId);
            Response? response = JsonSerializer.Deserialize<Response>(json);
            if (response.ErrorMessage != null)
            {
                throw new Exception(response.ErrorMessage);
            }

            return JsonSerializer.Deserialize<string>((JsonElement)(response.ReturnValue));
        }

        public List<TaskModel> GetColumnTasks(string email, string boardName, string columnName)
        {
            string json = serviceFactory.BoardService.GetColumnTasks(email, boardName, columnName);
            Response? response = JsonSerializer.Deserialize<Response>(json);
            if (response.ErrorMessage != null)
            {
                throw new Exception(response.ErrorMessage);
            }

            List<TaskToSend> tasks= JsonSerializer.Deserialize<List<TaskToSend>>((JsonElement)(response.ReturnValue));
            List<TaskModel> result= new List<TaskModel>();
            foreach (TaskToSend task in tasks)
            {
                result.Add(new TaskModel(this, task));
            }
            return result;
        }

    }
}
