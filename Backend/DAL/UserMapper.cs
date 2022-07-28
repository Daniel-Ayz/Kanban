using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DAL
{
    internal class UserMapper : Mapper
    {
        private const string TableName = "Users";
        public UserMapper() : base(TableName)
        {

        }

        internal List<UserDTO> SelectAllUsers()
        {
            List<UserDTO> result = Select().Cast<UserDTO>().ToList();

            return result;
        }

        public bool Insert(UserDTO user)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({UserDTO.EmailColumnName} , {UserDTO.PasswordColumnName}) " +
                        $"VALUES (@emailVal,@passwordVal);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.Email);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"passwordVal", user.Password);
                    
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
                    
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Fatal($"thrown exception: {ex.Message}, when trying to insert {user.Email} to database");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        protected override UserDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            UserDTO result = new UserDTO(reader.GetString(0), reader.GetString(1));
            return result;
        }
    }
}
