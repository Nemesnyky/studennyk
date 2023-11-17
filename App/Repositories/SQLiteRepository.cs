using Microsoft.Data.Sqlite;
using Task = App.Models.Task;

namespace App.Repositories
{
    public class SQLiteRepository : IRepository, IDisposable
    {
        private readonly SqliteConnection connection;
        private readonly SqliteCommand command;

        ///<summary>
        ///<para>If you need to open/create a database, use the following argument: "Data Source=name.db";</para>
        ///<para>If you need a temporary in-memory database, don't use any arguments;</para>
        ///</summary>
        public SQLiteRepository(string connectionString = "Data Source=:memory:")
        {
            connection = new SqliteConnection(connectionString);
            connection.Open();
            command = connection.CreateCommand();
            if (!TableExists("tasks"))
            {
                CreateTasksTable();
            }
        }

        private bool TableExists(string tableName)
        {
            command.CommandText = $"SELECT name FROM sqlite_master WHERE type = 'table' AND name = '{tableName}';";
            return command.ExecuteScalar() != null;
        }

        private void CreateTasksTable()
        {
            ExecuteSQLiteQuery(
                $"CREATE TABLE tasks (" +
                "task_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "title TEXT NOT NULL, " +
                "description TEXT NOT NULL, " +
                "created TEXT NOT NULL, " +
                "due TEXT, " +
                "is_done INT NOT NULL);");
        }

        #region Task CRUD
        public long AddTask(Task newTask)
        {
            ExecuteSQLiteQuery(
                $"INSERT INTO tasks (title, description, created, due, is_done) VALUES (" +
                $"'{newTask.Title}', " +
                $"'{newTask.Description}', " +
                $"'{newTask.Created:o}', " +
                $"'{newTask.Due:o}', " +
                $"{(newTask.IsDone ? 1 : 0)});"
                );

            return GetIdOfLastAddedTask();
        }

        public void DeleteTask(long task_id)
        {
            ExecuteSQLiteQuery($"DELETE FROM tasks WHERE task_id = {task_id};");
        }

        public void CompleteTask(long task_id)
        {
            ExecuteSQLiteQuery($"UPDATE tasks SET is_done = '{(Task.DONE ? 1 : 0)}' WHERE task_id = {task_id};");
        }

        public void UpdateTaskTitle(long task_id, string newTitle)
        {
            ExecuteSQLiteQuery($"UPDATE tasks SET title = '{newTitle}' WHERE task_id = {task_id};");
        }

        public void UpdateTaskDescription(long task_id, string newDescription)
        {
            ExecuteSQLiteQuery($"UPDATE tasks SET description = '{newDescription}' WHERE task_id = {task_id};");
        }

        public void UpdateTaskDueTime(long task_id, DateTimeOffset newDue)
        {
            ExecuteSQLiteQuery($"UPDATE tasks SET due = '{newDue:o}' WHERE task_id = {task_id};");
        }

        public Task GetTask(long task_id)
        {
            var tasks = GetTasksFromReader($"WHERE  task_id = {task_id}");

            return tasks.Any() ? tasks.First() : null;
        }

        public IEnumerable<Task> GetTasks()
        {
            return GetTasksFromReader();
        }
        #endregion

        private void ExecuteSQLiteQuery(string commandString)
        {
            command.CommandText = commandString;
            command.ExecuteNonQuery();
        }

        private long GetIdOfLastAddedTask()
        {
            command.CommandText = "SELECT last_insert_rowid()";
            return (long)command.ExecuteScalar();
        }

        private IEnumerable<Task> GetTasksFromReader(string where = "")
        {
            List<Task> tasks = new();
            command.CommandText = $"SELECT * FROM tasks {where};";
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                tasks.Add(new Task(
                    reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetDateTimeOffset(3),
                    reader.GetDateTimeOffset(4),
                    reader.GetBoolean(5))
                    );
            }

            return tasks;
        }

        public void Dispose()
        {
            command?.Dispose();
            connection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
