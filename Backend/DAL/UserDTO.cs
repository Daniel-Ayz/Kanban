namespace IntroSE.Kanban.Backend.DAL
{
    internal class UserDTO : DTO
    {
        internal const string EmailColumnName = "email";
        internal const string PasswordColumnName = "password";

        private readonly string _email;
        private string _password;

        internal string Email { get { return _email; } }
        internal string Password { get { return _password; } }

        internal UserDTO(string email, string password) : base(new UserMapper())
        {
            _email = email;
            _password = password;
        }

        internal void Persist()
        {
            ((UserMapper)mapper).Insert(this);
        }
    }
}
