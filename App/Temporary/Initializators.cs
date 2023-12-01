using App.Services;

namespace App.Temporary
{
    public static class Initializators
    {
        private static readonly Random random = new();
        public static RestService InitializeInMemoryDataBase(RestService client)
        {
            for (int i = 0; i < 3; i++)
            {
                Task.Run(async () => { await client.AddTask(Temporary.Generators.GenerateRandomTask()); });
            }

            return client;
        }
    }
}
