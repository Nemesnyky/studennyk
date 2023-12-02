using System;
using System.Collections.Generic;

namespace App.Models
{
    public class Task
    {
        public static readonly long DEFAULT_ID = -1;
        public const bool NOT_DONE = false;

        public long Id { get; set; } = DEFAULT_ID;
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Due { get; set; }
        public bool IsDone { get; set; }


        public Task(long id, string title, string description, DateTimeOffset created, DateTimeOffset due, bool isDone)
        {
            Id = id;
            Title = title;
            Description = description;
            Created = created;
            Due = due;
            IsDone = isDone;
        }

        public override bool Equals(object obj)
        {
            return obj is Task task &&
                   Id == task.Id &&
                   Title == task.Title &&
                   Description == task.Description &&
                   Created.Equals(task.Created) &&
                   Due.Equals(task.Due) &&
                   IsDone == task.IsDone;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title, Description, Created, Due, IsDone);
        }
    }

    public class TaskFactory
    {
        private long lastId = 0;
        private readonly Random random = new Random();

        private const string DefaultTitle = "Title";
        private const string DefaultDescription = "Description";
        private static readonly DateTimeOffset DefaultCreated = DateTimeOffset.UtcNow;
        private static readonly DateTimeOffset DefaultDue = DateTimeOffset.UtcNow.AddDays(7);

        private const bool DefaultIsDone = Task.NOT_DONE;

        public Task Generate(
            string title = DefaultTitle,
            string description = DefaultDescription,
            DateTimeOffset created = default,
            DateTimeOffset due = default,
            bool isDone = DefaultIsDone)
        {
            var taskId = random.Next(1, 100) + lastId;
            lastId = taskId;

            Task task = new Task(taskId, title, description,
                                created == default ? DefaultCreated : created,
                                due == default ? DefaultDue : due,
                                isDone);

            return task;
        }

        public List<Task> GenerateMany(int numberOfTasks,
                                       string title = DefaultTitle,
                                       string description = DefaultDescription,
                                       DateTimeOffset created = default,
                                       DateTimeOffset due = default,
                                       bool isDone = DefaultIsDone)
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < numberOfTasks; i++)
            {
                Task task = Generate(title, description, created, due, isDone);
                tasks.Add(task);
            }
            return tasks;
        }
    }
}
