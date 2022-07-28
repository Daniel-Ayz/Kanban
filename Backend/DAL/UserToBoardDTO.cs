namespace IntroSE.Kanban.Backend.DAL
{
    internal class UserToBoardDTO : DTO
    {
        internal const string EmailColumnName = "email";
        internal const string BoardIDColumnName = "boardID";

        private string _email;
        private int _boardID;

        internal string Email { get { return _email; } }
        internal int BoardID { get { return _boardID; } }

        internal UserToBoardDTO(string email, int boardID) : base(new UserToBoardMapper())
        {
            _email = email;
            _boardID= boardID; 
        }
    }
}
