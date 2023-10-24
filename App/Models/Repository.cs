using Microsoft.Data.Sqlite;

namespace DataBaseService
{
    interface IRepository
    {
        #region Permissions CRUD
        Permissions GetTaskPermissions(User user, User other, int task_id);
        void SetTaskPermissions(User user, User other, int task_id, Permissions perm);
        #endregion

        #region Task CRUD
        public void AddTask(User user, Task newTask);
        public void DeleteTask(User user, int task_id);
        public void UpdateTask(User user, int task_id, string newTitle, string newDescription, DateTimeOffset? newDue);
        public Task GetTask(User user, int task_id);
        IEnumerable<Task> GetTasks(User user);
        #endregion
    }

    public class SQLiteRepository : IDisposable, IRepository
    {
        private readonly SqliteConnection connection;
        public SQLiteRepository(string connectionString)
        {
            connection = new SqliteConnection(connectionString);
            connection.Open();
            CreateUsersTable();
            CreatePermsTable();
            AddUser(Constants.DEFAULT_USER);
        }

        #region Permission CRUD
        public Permissions GetTaskPermissions(User user, User other, int task_id)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM access_{user.Id} WHERE user_id = {other.Id} AND task_id = {task_id};";
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return (Permissions)reader.GetInt32(0);
                    }
                    else return Permissions.None;
                }
            }
        }
        public void SetTaskPermissions(User user, User other, int task_id, Permissions perm)
        {
            using (var command = connection.CreateCommand())
            {
                if (perm == Permissions.None)
                {
                    command.CommandText = $"DELETE FROM access{user.Id} WHERE user_id = {other.Id} AND task_id = {task_id};";
                    return;
                }
                command.CommandText = $"SELECT * FROM access_{user.Id} WHERE user_id = {other.Id} AND task_id = {task_id};";
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    command.CommandText = reader.Read()
                        ? $"UPDATE access_{user.Id} SET perm_id = {(int)perm} WHERE user_id = {other.Id} AND task_id = {task_id};"
                        : $"INSERT INTO access_{user.Id} (user_id, task_id, perm_id) VALUES ({other.Id}, {task_id}, {(int)perm});";
                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Task CRUD
        public void AddTask(User user, Task newTask)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"INSERT INTO tasks_{user.Id} (task_id, title, description, created, due, is_done) VALUES (" +
                    $"{newTask.Id}, " +
                    $"'{newTask.Title}', " +
                    $"'{newTask.Description}', " +
                    $"'{newTask.Created}', " +
                    $"'{newTask.Due}', " +
                    $"{newTask.IsDone});";
                command.ExecuteNonQuery();
            }
        }
        public void DeleteTask(User user, int task_id)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"DELETE FROM tasks_{user.Id} WHERE task_id = {task_id};";
                command.ExecuteNonQuery();
            }
        }
        public void UpdateTask(User user, int task_id, string newTitle = null, string newDescription = null, DateTimeOffset? newDue = null)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"UPDATE tasks_{user.Id} SET " +
                    (newTitle == null       ? "" : $"title = '{newTitle}', ") +
                    (newDescription == null ? "" : $"description = '{newDescription}', ") +
                    (newDue == null         ? "" : $"due = '{newDue}', ") +
                    $"created = '{DateTimeOffset.Now}' " +
                    $"WHERE task_id = {task_id};"; ;
                command.ExecuteNonQuery();
            }
        }
        public Task GetTask(User user, int task_id)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM tasks_{user.Id} WHERE task_id = {task_id};";
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Task
                        (
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            DateTimeOffset.Parse(reader.GetString(3)),
                            DateTimeOffset.Parse(reader.GetString(4)),
                            reader.GetBoolean(5)
                        );
                    }
                    else return null;
                }
            }
        }
        public IEnumerable<Task> GetTasks(User user)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM tasks_{user.Id};";
                List<Task> tasks = new List<Task>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new Task
                        (
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            DateTimeOffset.Parse(reader.GetString(3)),
                            DateTimeOffset.Parse(reader.GetString(4)),
                            reader.GetBoolean(5)
                        ));
                    }
                }
                return tasks;
            }
        }
        #endregion

        #region Table creators
        private void AddUser(User user)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"INSERT INTO users (user_id) VALUES ({user.Id});";
                command.ExecuteNonQuery();
            }
            CreateAccessTable(user);
            CreateTasksTable(user);
        }
        private void CreateUsersTable()
        {
            using (var command = connection.CreateCommand())
            {

                command.CommandText = $"SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'users';";
                if (command.ExecuteScalar() != null) return;

                command.CommandText = "CREATE TABLE users (user_id INT NOT NULL UNIQUE PRIMARY KEY);";
                command.ExecuteNonQuery();
            }
        }
        private void CreatePermsTable()
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='perms'";
                if (command.ExecuteScalar() != null) return;

                command.CommandText = "CREATE TABLE perms (" +
                    "perm_id INT NOT NULL UNIQUE, " +
                    "label TEXT NOT NULL UNIQUE)";
                command.ExecuteNonQuery();

                command.CommandText = $"INSERT INTO perms (perm_id, label) VALUES (1, 'ReadOnly'), (2, 'ReadWrite');";
                command.ExecuteNonQuery();
            }
        }
        private void CreateAccessTable(User user)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='access_{user.Id}';";
                if (command.ExecuteScalar() != null) return;

                command.CommandText =
                    $"CREATE TABLE access_{user.Id}(" +
                    $"user_id INT NOT NULL UNIQUE, " +
                    $"task_id INT NOT NULL UNIQUE, " +
                    $"perm_id INT NOT NULL, " +
                    $"FOREIGN KEY(user_id) REFERENCES users(user_id), " +
                    //$"FOREIGN KEY(task_id) REFERENCES tasks(task_id), " +
                    $"FOREIGN KEY(perm_id) REFERENCES perms(perm_id));";
                command.ExecuteNonQuery();
            }
        }
        private void CreateTasksTable(User user)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='tasks_{user.Id}';";
                if (command.ExecuteScalar() != null) return;

                command.CommandText =
                    $"CREATE TABLE tasks_{user.Id}(" +
                    "task_id INT NOT NULL PRIMARY KEY UNIQUE, " +
                    "title TEXT NOT NULL, " +
                    "description TEXT NOT NULL, " +
                    "created TEXT NOT NULL, " +
                    "due TEXT, " +
                    "is_done INT NOT NULL);";
                command.ExecuteNonQuery();
            }
        }
        #endregion
        
        public void Dispose()
        {
            connection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
