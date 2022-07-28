using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DAL
{
    internal class TaskMapper : Mapper
    {
        private const string TableName = "Tasks";
        internal TaskMapper() : base(TableName)

        {

        }


        internal List<TaskDTO> SelectAllTasks()
        {
            List<TaskDTO> result = Select().Cast<TaskDTO>().ToList();

            return result;
        }


        // update description and title
        internal bool Update(long taskid, long boardid, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TableName} set [{attributeName}]='{attributeValue}' where {TaskDTO.taskIDcolumnName}={taskid} AND {TaskDTO.boardIDcolumnName}={boardid}"
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

        // update duedate
        
        internal bool Update(long taskid, long boardid, string attributeName, DateTime attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {TableName} set [{attributeName}]=@date where {TaskDTO.taskIDcolumnName}={taskid} AND {TaskDTO.boardIDcolumnName}={boardid}"
                };
                try
                {
                    command.Parameters.AddWithValue("@date", attributeValue);
                    command.Parameters.Add(new SQLiteParameter(attributeName,attributeValue));
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
        

        internal bool Delete(TaskDTO DTOObj)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {TableName} where {TaskDTO.taskIDcolumnName}={DTOObj.TaskId} AND {TaskDTO.boardIDcolumnName}= {DTOObj.BoardID}"
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

        internal bool Insert(TaskDTO task)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TableName} ({TaskDTO.taskIDcolumnName} , {TaskDTO.boardIDcolumnName}, {TaskDTO.columnNamecolumnName},{TaskDTO.descriptioncolumnName}, {TaskDTO.assigneeEmailcolumnName} , {TaskDTO.titlecolumnName}, {TaskDTO.creationTimecolumnName}, {TaskDTO.dueDatecolumnName}) " +
                        $"VALUES (@taskidVal,@boardidval, @columnnameval, @descriptionval, @assigneeemailval, @titleval, @creationtimeval, @duedateval);";

                    SQLiteParameter taskidParam = new SQLiteParameter(@"taskidVal", task.TaskId);
                    SQLiteParameter boardidParam = new SQLiteParameter(@"boardidVal", task.BoardID);
                    SQLiteParameter columnNameParam = new SQLiteParameter(@"columnnameval", task.ColumnName);
                    SQLiteParameter duedateidParam = new SQLiteParameter(@"duedateval", task.DueDate);
                    SQLiteParameter creationtimeidParam = new SQLiteParameter(@"creationtimeval", task.CreationTime);
                    SQLiteParameter assigneeemailParam = new SQLiteParameter(@"assigneeemailval", task.AssigneeEmail);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"descriptionval", task.Description);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleval", task.Title);

                    command.Parameters.Add(taskidParam);
                    command.Parameters.Add(boardidParam);
                    command.Parameters.Add(duedateidParam);
                    command.Parameters.Add(creationtimeidParam);
                    command.Parameters.Add(assigneeemailParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(columnNameParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Fatal($"thrown exception: {ex.Message}, when trying to insert {task} to database");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }
 
        protected override TaskDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            TaskDTO result = new TaskDTO((int)((long)reader.GetValue(0)), (int)((long)reader.GetValue(1)), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), DateTime.Parse(reader.GetString(6)), DateTime.Parse(reader.GetString(7)));
            return result;
        }
    }
}
