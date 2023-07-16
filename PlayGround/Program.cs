

string dirName = @"D:\Завантаження";

if (Directory.Exists(dirName))
{
    string[] dirs = Directory.GetDirectories(dirName);

	foreach (var dir in dirs)
	{
		Console.WriteLine(dir);
	}
}
