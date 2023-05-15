using System.Drawing;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

CancellationToken token = cancelTokenSource.Token;

Task task1 = Task.Run(() => ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File1.txt", (new Random()).Next(1, 10)), token);
Task task2 = Task.Run(() => ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File2.txt", (new Random()).Next(1, 10)), token);
Task task3 = Task.Run(() => ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File3.txt", (new Random()).Next(1, 10)), token);
Task task4 = Task.Run(() => ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File4.txt", (new Random()).Next(1, 10)), token);
Task task5 = Task.Run(() => ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File5.txt", (new Random()).Next(1, 10)), token);

List<Task> tasks = new List<Task>() { task1, task2, task3, task4, task5 };

//for (int i = 1; i < 6; i++)
//{
//    tasks.Add(new Task(() => ReadAndPrintAsync
//    ($@"D:\\QA\\C#\\Hillel\\HomeWork5\\File{i}.txt", (new Random()).Next(0, 15)), token));
//}

try
{
    foreach (var t in tasks)
    {
        //t.Start();
    }

    Task.WaitAny(tasks.ToArray());
    cancelTokenSource.Cancel();
    Task.WaitAll(tasks.ToArray());
}

catch (AggregateException theException)
{
    foreach (Exception e in theException.InnerExceptions)
    {
        if (e is TaskCanceledException)
            Console.WriteLine("Operation aborted");
        else
            Console.WriteLine(e.Message);
    }
}

//foreach (var t in tasks)
//{
//    Console.WriteLine($"Task Status: {t.Status}");
//}

//-------------------------------------------------------

async Task ReadAndPrintAsync(string path, int color)
{
    using (StreamReader reader = new StreamReader(path))
    {
        string? line;

        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (token.IsCancellationRequested) return;

            lock (Console.Out)
            {
                Console.ForegroundColor = (ConsoleColor)color;
                Console.WriteLine(line);
                Console.ResetColor();
            }
        }
    }
}


