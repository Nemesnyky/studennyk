using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

namespace App.Services
{
    public class SQLiteRepository : IRepository, IDisposable
    {
        private readonly SqliteConnection connection;
        private readonly SqliteCommand command;

        ///<summary>
        ///If you need a temporary database, use the following argument: "Data Source=:memory:"
        ///</summary>
        public SQLiteRepository(string connectionString)
        {
            connection = new SqliteConnection(connectionString);
            connection.Open();
            command = connection.CreateCommand();
            if (!TableExists("tasks"))
                CreateTasksTable();
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
        public int AddTask(Task newTask)
        {
            ExecuteSQLiteQuery(
                $"INSERT INTO tasks (title, description, created, due, is_done) VALUES (" +
                $"'{newTask.Title}', " +
                $"'{newTask.Description}', " +
                $"'{newTask.Created}', " +
                $"'{newTask.Due}', " +
                $"{newTask.IsDone});"
                );

            return GetIdOfLastAddedTask();
        }

        public void DeleteTask(int task_id)
        {
            ExecuteSQLiteQuery($"DELETE FROM tasks WHERE task_id = {task_id};");
        }

        public void UpdateTaskTitle(int task_id, string newTitle)
        {
            ExecuteSQLiteQuery($"UPDATE tasks SET title = '{newTitle}' WHERE task_id = {task_id};");
        }

        public void UpdateTaskDescription(int task_id, string newDescription)
        {
            ExecuteSQLiteQuery($"UPDATE tasks SET description = '{newDescription}' WHERE task_id = {task_id};");
        }

        public void UpdateTaskDueTime(int task_id, DateTimeOffset newDue)
        {
            ExecuteSQLiteQuery($"UPDATE tasks SET due = '{newDue}' WHERE task_id = {task_id};");
        }

        public Task GetTask(int task_id)
        {
            IEnumerable<Task> tasks = GetTasksFromReader($"WHERE  task_id = {task_id}");

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

        private int GetIdOfLastAddedTask()
        {
            command.CommandText = "SELECT last_insert_rowid()";
            return (int)(long)command.ExecuteScalar();
        }

        private IEnumerable<Task> GetTasksFromReader(string where = "")
        {
            var tasks = new List<Task>();
            command.CommandText = $"SELECT * FROM tasks {where};";
            using var reader = command.ExecuteReader();

            while (reader.Read())
                tasks.Add(new Task(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    DateTimeOffset.Parse(reader.GetString(3)),
                    DateTimeOffset.Parse(reader.GetString(4)),
                    reader.GetBoolean(5))
                    );

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
