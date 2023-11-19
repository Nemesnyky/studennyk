using App.Repositories;

namespace App.Temporary
{
    public static class Initializators
    {
        private static readonly Random random = new();
        public static IRepository InitializeInMemoryDataBase(IRepository repository)
        {
            int numTasks = random.Next(5, 12);
            for (int i = 0; i < numTasks; i++)
                repository.AddTask(Temporary.Generators.GenerateRandomTask());

            return repository;
        }
    }
}
