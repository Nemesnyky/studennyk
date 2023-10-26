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
            _task.Id = _repository.AddTask(_user, _task);
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

            Task actual = _repository.GetTask(_user, _task.Id);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeleteTaskTest()
        {
            Task? expected = null;

            _repository.DeleteTask(_user, _task.Id);
            Task actual = _repository.GetTask(_user, _task.Id);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateTaskTitleTest()
        {
            string expected = "newTitle";

            _repository.UpdateTaskTitle(_user, _task.Id, "newTitle");
            string actual = _repository.GetTask(_user, _task.Id).Title;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateTaskDescriptionTest()
        {
            string expected = "newDescription";

            _repository.UpdateTaskDescription(_user, _task.Id, "newDescription");
            string actual = _repository.GetTask(_user, _task.Id).Description;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateTaskDueTimeTest()
        {
            DateTimeOffset expected = DateTimeOffset.Now;

            _repository.UpdateTaskDueTime(_user, _task.Id, expected);
            DateTimeOffset actual = _repository.GetTask(_user, _task.Id).Due;

            Assert.Equal(expected.ToString(), actual.ToString());
        }

        [Fact]
        public void GetTasksTest()
        {
            List<Task> expected = new List<Task> { _task, Common.GenerateRandomTask(), Common.GenerateRandomTask() };

            expected[1].Id = _repository.AddTask(_user, expected[1]);
            expected[2].Id = _repository.AddTask(_user, expected[2]);
            List<Task> actual = (List<Task>)_repository.GetTasks(_user);

            Assert.True(expected.SequenceEqual(actual));
        }
    }
}
