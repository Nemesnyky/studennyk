using App.Repositories;
using Task = App.Models.Task;

namespace AppTests.Services
{
    public class SQLiteRepositoryTests : IDisposable
    {
        private readonly SQLiteRepository repository;
        private readonly Task task;

        public SQLiteRepositoryTests()
        {
            repository = new SQLiteRepository("Data Source=:memory:");
            task = App.Temporary.Generators.GenerateRandomTask();
            task.Id = repository.AddTask(task);
        }

        public void Dispose()
        {
            repository.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void ShouldHaveTheTask()
        {
            Task expected = task;

            Task actual = repository.GetTask(task.Id);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldDeleteTask()
        {
            Task? expected = null;

            repository.DeleteTask(task.Id);
            Task actual = repository.GetTask(task.Id);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldDoneTask()
        {
            bool expected = Task.DONE;

            repository.DoneTask(task.Id);
            bool actual = repository.GetTask(task.Id).IsDone;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldUpdateTaskTitle()
        {
            string expected = "newTitle";

            repository.UpdateTaskTitle(task.Id, "newTitle");
            string actual = repository.GetTask(task.Id).Title;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldUpdateTaskDescription()
        {
            string expected = "newDescription";

            repository.UpdateTaskDescription(task.Id, "newDescription");
            string actual = repository.GetTask(task.Id).Description;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldUpdateTaskDueTime()
        {
            DateTimeOffset expected = DateTimeOffset.Now;

            repository.UpdateTaskDueTime(task.Id, expected);
            DateTimeOffset actual = repository.GetTask(task.Id).Due;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldGetTasks()
        {
            var expected = new List<Task> {
                task,
                App.Temporary.Generators.GenerateRandomTask(),
                App.Temporary.Generators.GenerateRandomTask()
            };

            expected[1].Id = repository.AddTask(expected[1]);
            expected[2].Id = repository.AddTask(expected[2]);
            IEnumerable<Task> actual = repository.GetTasks();

            Assert.True(expected.SequenceEqual(actual));
        }
    }
}
