//using Internal;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

static class Program
{
	static async Task Main(string[] args)
	{
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        Console.WriteLine("===========================================");
		Console.WriteLine("         Main Method Started");
        Console.WriteLine("===========================================");
		AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
		
		var task1 = Task.Run(async () =>
		{
			Console.WriteLine("Task 1 started");
			Console.WriteLine("Task 1 menunggu Task 2 meng-edit gambar ke 2");
             Thread.Sleep(5000);
			await Task.Run(() => _autoResetEvent.WaitOne());
            Thread.Sleep(5000);
			Console.WriteLine("Task 1 received signal");
			await ProcessImageAsync1(); // MyMethod
		});

        Thread.Sleep(5000);

		var task2 = Task.Run(() =>
		{
			Console.WriteLine("Task 2 started");
			ProcessImageAsync2(); // MyMethod
			Console.WriteLine("Task 2 completed work");
             Thread.Sleep(5000);
			_autoResetEvent.Set();
			Console.WriteLine("Task 2 memberikan sinyal ke Task 1 bahwa gambar ke 2 sudah di edit");
		});
        Thread.Sleep(2000);
		await Task.WhenAll(task1, task2);
        
		Console.WriteLine("Main Method Completed");
        stopwatch.Stop();
        Console.WriteLine("===========================================");
        Console.WriteLine($"Program complete. Elapsed time: {stopwatch.ElapsedMilliseconds}ms");
        Console.WriteLine("===========================================");

	}
     public static async Task ProcessImageAsync1()
    {
        Console.WriteLine("menjalankan program edit gambar ke 1");
        using (var image = Image.Load("C:/Users/Formulatrix/Desktop/need purchase lagi.png"))
        // pakai using biar langung ke dispose()
        {
            image.Mutate(x => x.Resize(new ResizeOptions { Size = new Size(800, 600) }));
            await image.SaveAsync("output.jpg");
            Thread.Sleep(5000);
        }
        Console.WriteLine("============================");
        Console.WriteLine("edit gambar ke 1 done");
        Console.WriteLine("============================");
    }

     public static async Task ProcessImageAsync2()
    {
        Console.WriteLine("menjalankan program edit gambar ke 2");
        using (var image2 = Image.Load("C:/Users/Formulatrix/Desktop/need purchase lagi.png"))
        {
            image2.Mutate(x => x.Grayscale());
            await image2.SaveAsync("outputgreyscale.jpg");
            
        }
        Console.WriteLine("============================");
        Console.WriteLine("edit gambar ke 2 done");
        Console.WriteLine("============================");
    }
}
