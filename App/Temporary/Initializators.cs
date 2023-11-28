using App.Services;

namespace App.Temporary
{
    public static class Initializators
    {
        private static readonly Random random = new();
        public static RestService InitializeInMemoryDataBase(RestService client)
        {
            Task.Run(async () => { await client.AddTask(Temporary.Generators.GenerateRandomTask()); await Task.Delay(100); });

            return client;
        }
    }
}
