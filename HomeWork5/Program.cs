using System.Drawing;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;



//var file1 = ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\FIle1.txt", 5);

//var file2 = ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File2.txt", 7);

//var file3 = ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File3.txt", 12);

//var file4 = ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File4.txt", 3);

//var file5 = ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File5.txt", 9);

//await file1;

//await file2;

//await file3;

//await file4;

//await file5;

CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

CancellationToken token = cancelTokenSource.Token;

Task task1 = new Task(() => ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\FIle1.txt", 5), token);
Task task2 = new Task(() => ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File2.txt", 7), token);
Task task3 = new Task(() => ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File3.txt", 12), token);
Task task4 = new Task(() => ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File4.txt", 3), token);
Task task5 = new Task(() => ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File5.txt", 9), token);

List<Task> tasks = new List<Task>() { task1, task2, task3, task4, task5 };

try
{
    foreach (var t in tasks)
    {
        t.Start();

        if (t.IsCompleted) //== TaskStatus.RanToCompletion
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

Console.WriteLine($"Task Status: {task1.Status}");
Console.WriteLine($"Task Status: {task2.Status}");
Console.WriteLine($"Task Status: {task3.Status}");
Console.WriteLine($"Task Status: {task4.Status}");
Console.WriteLine($"Task Status: {task5.Status}");

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


 