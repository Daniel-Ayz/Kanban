using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DAL
{
    internal class BoardMapper : Mapper
    {
        private const string MessageTableName = "Boards";
        private UserToBoardMapper userToBoard;

        internal BoardMapper() : base(MessageTableName)
        {
            userToBoard = new UserToBoardMapper();
        }


        internal List<BoardDTO> SelectAllBoards()
        {
            List<BoardDTO> result = Select().Cast<BoardDTO>().ToList();
            return result;
        }


        internal bool Insert(BoardDTO board)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {MessageTableName} ({BoardDTO.boardIdColumnName} ,{BoardDTO.boardNameColumnName},{BoardDTO.boardOwnerEmailColumnName}) " +
                        $"VALUES (@idVal,@nameVal,@ownerVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", board.Id);
                    SQLiteParameter nameParam = new SQLiteParameter(@"nameVal", board.Name);
                    SQLiteParameter ownerParam = new SQLiteParameter(@"ownerVal", board.OwnerEmail);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(ownerParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    log.Fatal($"thrown exception: {ex.Message}, when trying to insert {board.Id},{board.Name} to database");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        internal bool Update(long id, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {MessageTableName} set [{attributeName}]='{attributeValue}' where {BoardDTO.boardIdColumnName}={id}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Fatal($"thrown exception: {ex.Message}, when trying to update board: {id},{attributeName} to database");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        internal bool Delete(BoardDTO board)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {MessageTableName} where {BoardDTO.boardIdColumnName}={board.Id}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        internal bool JoinBoard(string email, int boardID)
        {
            
            return userToBoard.Insert(email, boardID);
        }

        internal bool LeaveBoard(string email, int boardID)
        {
            return userToBoard.Delete(email, boardID);
        }


        protected override BoardDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            BoardDTO result = new BoardDTO((int)((long)reader.GetValue(0)), reader.GetString(1), reader.GetString(2));
            return result;
        }

    }
}
