int result = Rank(3, new int[] {1, 2, 3, 4, 5, 6, 7, 8});

Console.WriteLine(result);

int Rank(int key, int[] numbers)
{
    int low = 0;
    int high = numbers.Length - 1;

    while (low <= high)
    {        
        int mid = low + (high - low) / 2;
        
        if (key < numbers[mid]) high = mid - 1;

        else if (key > numbers[mid]) low = mid + 1;

        else return mid;
    }
    return -1;
}