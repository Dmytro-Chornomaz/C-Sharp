﻿using System.Runtime.CompilerServices;

var file1 = ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\FIle1.txt", 5);

var file2 = ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File2.txt", 7);

var file3 = ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File3.txt", 12);

var file4 = ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File4.txt", 3);

var file5 = ReadAndPrintAsync(@"D:\\QA\\C#\\Hillel\\HomeWork5\\File5.txt", 9);

await file1;

await file2;

await file3;

await file4;

await file5;

async Task ReadAndPrintAsync(string path, int color)
{
    string pathText = path;
    
    int textColor = color;

    await Task.Delay(1000);

    IEnumerable<string> lines = File.ReadLines(pathText);

    Console.ForegroundColor = (ConsoleColor)textColor;

    Console.WriteLine(String.Join(Environment.NewLine, lines));

    Console.ResetColor();

}