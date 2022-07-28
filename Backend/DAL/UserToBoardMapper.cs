using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DAL
{
    internal class UserToBoardMapper : Mapper
    {
        private const string TableName = "UserToBoard";

        public UserToBoardMapper() : base(TableName)
        {

        }

        internal List<UserToBoardDTO> SelectAllUserToBoard()
        {
            List<UserToBoardDTO> result = Select().Cast<UserToBoardDTO>().ToList();

            return result;
        }

        internal bool Insert(string email, int boardId)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({UserToBoardDTO.EmailColumnName} ,{UserToBoardDTO.BoardIDColumnName}) " +
                        $"VALUES (@emailVal,@boardIDVal);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", email);
                    SQLiteParameter boardidParam = new SQLiteParameter(@"boardidVal", boardId);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(boardidParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Fatal($"thrown exception: {ex.Message}, when trying to insert {email}, {boardId} to database");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        internal bool Delete(string email, int boardId)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {TableName} where {UserToBoardDTO.EmailColumnName}= '{email}' AND {UserToBoardDTO.BoardIDColumnName}= {boardId}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Fatal($"thrown exception: {ex.Message}, when trying to delete {email}, {boardId} from database");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        protected override UserToBoardDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            UserToBoardDTO result = new UserToBoardDTO(reader.GetString(0), (int)((long)reader.GetValue(1)));
            return result;
        }
    }
}
