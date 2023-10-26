using Microsoft.Data.Sqlite;

namespace App.Services
{
    public class SQLiteRepository : IRepository, IDisposable
    {
        private readonly SqliteConnection connection;
        private readonly SqliteCommand command;
        public SQLiteRepository(string connectionString)
        {
            connection = new SqliteConnection(connectionString);
            connection.Open();
            command = connection.CreateCommand();
            CreateUsersTable();
            AddUser(Common.DEFAULT_USER);
        }

        #region Task CRUD
        public int AddTask(User user, Task newTask)
        {
            ExecuteSQLiteCommand(
                $"INSERT INTO tasks_{user.Id} (title, description, created, due, is_done) VALUES (" +
                $"'{newTask.Title}', " +
                $"'{newTask.Description}', " +
                $"'{newTask.Created}', " +
                $"'{newTask.Due}', " +
                $"{newTask.IsDone});");
            command.CommandText = "SELECT last_insert_rowid()";
            return (int)(long)command.ExecuteScalar();
        }
        public void DeleteTask(User user, int task_id)
        {
            ExecuteSQLiteCommand($"DELETE FROM tasks_{user.Id} WHERE task_id = {task_id};");
        }
        public void UpdateTaskTitle(User user, int task_id, string newTitle)
        {
            ExecuteSQLiteCommand($"UPDATE tasks_{user.Id} SET title = '{newTitle}' WHERE task_id = {task_id};");
        }
        public void UpdateTaskDescription(User user, int task_id, string newDescription)
        {
            ExecuteSQLiteCommand($"UPDATE tasks_{user.Id} SET description = '{newDescription}' WHERE task_id = {task_id};");
        }
        public void UpdateTaskDueTime(User user, int task_id, DateTimeOffset newDue)
        {
            ExecuteSQLiteCommand($"UPDATE tasks_{user.Id} SET due = '{newDue}' WHERE task_id = {task_id};");
        }
        public Task GetTask(User user, int task_id)
        {
            command.CommandText = $"SELECT * FROM tasks_{user.Id} WHERE task_id = {task_id};";
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Task(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        DateTimeOffset.Parse(reader.GetString(3)),
                        DateTimeOffset.Parse(reader.GetString(4)),
                        reader.GetBoolean(5));
                }
                else return null;
            }
        }
        public IEnumerable<Task> GetTasks(User user)
        {
            List<Task> tasks = new List<Task>();
            command.CommandText = $"SELECT * FROM tasks_{user.Id};";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tasks.Add(new Task(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        DateTimeOffset.Parse(reader.GetString(3)),
                        DateTimeOffset.Parse(reader.GetString(4)),
                        reader.GetBoolean(5)));
                }
                return tasks;
            }
        }
        #endregion

        private void AddUser(User user)
        {
            ExecuteSQLiteCommand($"INSERT INTO users (user_id) VALUES ({user.Id});");
            CreateTasksTable(user);
        }
        private void CreateUsersTable()
        {
            if (TableExists("users")) return;
            ExecuteSQLiteCommand("CREATE TABLE users (user_id INT NOT NULL UNIQUE PRIMARY KEY);");
        }
        private void CreateTasksTable(User user)
        {
            if (TableExists($"tasks_{user.Id}")) return;
            ExecuteSQLiteCommand(
                $"CREATE TABLE tasks_{user.Id} (" +
                "task_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                "title TEXT NOT NULL, " +
                "description TEXT NOT NULL, " +
                "created TEXT NOT NULL, " +
                "due TEXT, " +
                "is_done INT NOT NULL);");
        }

        private void ExecuteSQLiteCommand(string commandString)
        {
            command.CommandText = commandString;
            command.ExecuteNonQuery();
        }
        private bool TableExists(string tableName)
        {
            command.CommandText = $"SELECT name FROM sqlite_master WHERE type = 'table' AND name = '{tableName}';";
            return command.ExecuteScalar() != null;
        }
        public void Dispose()
        {
            command?.Dispose();
            connection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
