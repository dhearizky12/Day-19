using System;
using System.Threading;
using System.Threading.Tasks;

static class Program
{
    static int counter = 0;
    static object counterLock = new object();

    static async Task Main(string[] args)
    {
        var task1 = IncrementCounterAsync();
        var task2 = IncrementCounterAsync();

        await Task.WhenAll(task1, task2);

        Console.WriteLine($"Counter: {counter}");
        Console.ReadKey();
    }

    static async Task IncrementCounterAsync()
    {
        for (int i = 0; i < 10; i++)
        {
            bool lockTaken = false;

            try
            {
                Monitor.Enter(counterLock, ref lockTaken);
                counter++;
                Console.WriteLine($"Counter from: {counter}");
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(counterLock);
                }
            }

            await Task.Delay(100);
        }
    }
}
