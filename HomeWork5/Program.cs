using System.Drawing;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

CancellationToken token = cancelTokenSource.Token;

Task task1 = new Task(() => ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File1.txt", (new Random()).Next(0, 15)), token);
Task task2 = new Task(() => ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File2.txt", (new Random()).Next(0, 15)), token);
Task task3 = new Task(() => ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File3.txt", (new Random()).Next(0, 15)), token);
Task task4 = new Task(() => ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File4.txt", (new Random()).Next(0, 15)), token);
Task task5 = new Task(() => ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File5.txt", (new Random()).Next(0, 15)), token);

List<Task> tasks = new List<Task>(){ task1, task2, task3, task4, task5 };

//for (int i = 1; i < 6; i++)
//{
//    tasks.Add(new Task(() => ReadAndPrintAsync
//    ($@"D:\\QA\\C#\\Hillel\\HomeWork5\\File{i}.txt", (new Random()).Next(0, 15)), token));
//}

try
{
    foreach (var t in tasks)
    {
        t.Start();

        Thread.Sleep(1000);

        if (t.IsCompleted)
        {
            cancelTokenSource.Cancel();
            
        }

        t.Wait();
    }

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

foreach (var t in tasks)
{
    Console.WriteLine($"Task Status: {t.Status}");
}

//-------------------------------------------------------

async Task ReadAndPrintAsync(string path, int color)
{
    if (token.IsCancellationRequested)
    { 
        token.ThrowIfCancellationRequested();        
    }

    Console.ForegroundColor = (ConsoleColor)color;

    using (StreamReader reader = new StreamReader(path))
    {
        string? line;

        while ((line = await reader.ReadLineAsync()) != null)
        {
            Console.WriteLine(line);
        }
    }

    Console.ResetColor();

}


 