using App.Temporary;
using App.Services;
using Task = App.Models.Task;

namespace AppTests.Services
{
    public class SQLiteRepositoryTests : IDisposable
    {
        private readonly RestService client;
        private readonly Task task;

        public SQLiteRepositoryTests()
        {
            client = new RestService();
            System.Threading.Tasks.Task.Run(async () =>
            {
                await client.RefreshDataAsync();
                task = Generators.GenerateRandomTask();
                task.Id = await client.AddTask(task);
            });
        }

        public void Dispose()
        {
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

            client.DeleteTask(task.Id);
            Task actual = client.GetTask(task.Id);

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
        public void ShouldDoneTask()
        {
            bool expected = Task.DONE;

            repository.UpdateTaskStatus(task.Id, Task.DONE);
            bool actual = repository.GetTask(task.Id).IsDone;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldGetTasks()
        {
            var expected = new List<Task> {
                task,
                Generators.GenerateRandomTask(),
                Generators.GenerateRandomTask()
            };

            expected[1].Id = repository.AddTask(expected[1]);
            expected[2].Id = repository.AddTask(expected[2]);
            IEnumerable<Task> actual = repository.GetTasks();

            Assert.True(expected.SequenceEqual(actual));
        }
    }
}
