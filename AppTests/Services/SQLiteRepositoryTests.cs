using App.Services;

namespace AppTests.Services
{
    public class SQLiteRepositoryTests : IDisposable
    {
        private readonly SQLiteRepository _repository;
        private readonly User _user;
        private readonly Task _task;

        public SQLiteRepositoryTests()
        {
            _repository = new SQLiteRepository("Data Source=:memory:");
            _user = Common.DEFAULT_USER;
            _task = Common.GenerateRandomTask();
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void AddTaskTest()
        {
            Task expected = _task;

            expected.Id = _repository.AddTask(_user, _task);
            Task actual = _repository.GetTask(_user, expected.Id);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeleteTaskTest()
        {
            Task? expected = null;

            int _task_id = _repository.AddTask(_user, _task);
            _repository.DeleteTask(_user, _task_id);
            Task actual = _repository.GetTask(_user, _task_id);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateTaskTitleTest()
        {
            string expected = "newTitle";

            int _task_id = _repository.AddTask(_user, _task);
            _repository.UpdateTaskTitle(_user, _task_id, "newTitle");
            string actual = _repository.GetTask(_user, _task_id).Title;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateTaskDescriptionTest()
        {
            string expected = "newDescription";

            int _task_id = _repository.AddTask(_user, _task);
            _repository.UpdateTaskDescription(_user, _task_id, "newDescription");
            string actual = _repository.GetTask(_user, _task_id).Description;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateTaskDueTimeTest()
        {
            DateTimeOffset expected = DateTimeOffset.Now;

            int _task_id = _repository.AddTask(_user, _task);
            _repository.UpdateTaskDueTime(_user, _task_id, expected);
            DateTimeOffset actual = _repository.GetTask(_user, _task_id).Due;

            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public void GetTasksTest()
        {
            List<Task> expected = new List<Task> { Common.GenerateRandomTask(), Common.GenerateRandomTask(), Common.GenerateRandomTask() };

            expected[0].Id = _repository.AddTask(_user, expected[0]);
            expected[1].Id = _repository.AddTask(_user, expected[1]);
            expected[2].Id = _repository.AddTask(_user, expected[2]);
            List<Task> actual = (List<Task>)_repository.GetTasks(_user);

            Assert.True(expected.SequenceEqual(actual));
        }
    }
}
