using App.Temporary;
using App.Services;
using Task = App.Models.Task;

namespace AppTests.Api
{
    public class ApiTests : IDisposable
    {
        private readonly RestService client;
        private Task task;

        public ApiTests()
        {
            client = new RestService();
            task = Generators.GenerateRandomTask();
            task.Id = client.AddTask(task).Result;
        }

        public async void Dispose()
        {
            await client.DeleteTask(task.Id);
        }

        [Fact]
        public async void ShouldHaveTheTask()
        {
            var expected = task;

            var actual = await client.GetTask(task.Id);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void ShouldDeleteTask()
        {
            Task? expected = null;

            await client.DeleteTask(task.Id);
            Task actual = await client.GetTask(task.Id);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void ShouldUpdateTaskTitle()
        {
            string expected = "newTitle";

            await client.UpdateTaskTitle(task.Id, "newTitle");
            string actual = (await client.GetTask(task.Id)).Title;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void ShouldUpdateTaskDescription()
        {
            string expected = "newDescription";

            await client.UpdateTaskDescription(task.Id, "newDescription");
            string actual = (await client.GetTask(task.Id)).Description;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void ShouldUpdateTaskDueTime()
        {
            DateTimeOffset expected = DateTimeOffset.Now;

            await client.UpdateTaskDue(task.Id, expected);
            DateTimeOffset actual = (await client.GetTask(task.Id)).Due;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void ShouldDoneTask()
        {
            bool expected = Task.DONE;

            await client.UpdateTaskStatus(task.Id, Task.DONE);
            bool actual = (await client.GetTask(task.Id)).IsDone;

            Assert.Equal(expected, actual);
        }

    }
}
