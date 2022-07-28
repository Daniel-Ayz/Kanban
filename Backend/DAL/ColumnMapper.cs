using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DAL
{
    internal class ColumnMapper : Mapper
    {
        private const string TableName = "Columns";
        internal ColumnMapper() : base(TableName)
        {

        }


        internal List<ColumnDTO> SelectAllColumns()
        {
            List<ColumnDTO> result = Select().Cast<ColumnDTO>().ToList();

            return result;
        }


        internal bool Update(long boardid, string columnName, string attributeName, long attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TableName} set [{attributeName}]='{attributeValue}' where {ColumnDTO.boardIDcolumnName}={boardid} AND {ColumnDTO.columnNamecolumnName}='{columnName}'"

                };
                try
                {

                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Fatal($"thrown exception: {ex.Message}, when trying to update {attributeValue} to database");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        

        internal bool Delete(ColumnDTO DTOObj)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {TableName} where {ColumnDTO.boardIDcolumnName}={DTOObj.BoardID} AND {ColumnDTO.columnNamecolumnName}= '{DTOObj.ColumnName}'"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Fatal($"thrown exception: {ex.Message}, when trying to delete {DTOObj} from database");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        internal bool Insert(ColumnDTO column)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({ColumnDTO.boardIDcolumnName}, {ColumnDTO.columnNamecolumnName}, {ColumnDTO.maxTaskscolumnName}) " +
                        $"VALUES (@boardidval, @columnnameval, @maxtasksval);";

                    SQLiteParameter boardidParam = new SQLiteParameter(@"boardidVal",column.BoardID );
                    SQLiteParameter columnnameParam = new SQLiteParameter(@"columnnameval", column.ColumnName);
                    SQLiteParameter maxtasksParam = new SQLiteParameter(@"maxtasksval", column.MaxTasks);
                    

                    command.Parameters.Add(boardidParam);
                    command.Parameters.Add(columnnameParam);
                    command.Parameters.Add(maxtasksParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Fatal($"thrown exception: {ex.Message}, when trying to insert {column} to database");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        protected override ColumnDTO ConvertReaderToObject(SQLiteDataReader reader)

        {
            ColumnDTO result = new ColumnDTO((int)((long)reader.GetValue(0)), reader.GetString(1),(int)((long)reader.GetValue(2)));
            return result;
        }
    }
}
