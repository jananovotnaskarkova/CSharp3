namespace Greed;

public class Play
{
    private static List<int> RollList;
    private int Result;

    public Play(List<int> rolllist, int result)
    {
        RollList = rolllist;
        Result = result;
    }

    public static List<int> Roll(int diceCount)
    {
        Random random = new();
        List<int> roll_list = [];

        for (int i = 0; i < diceCount; i++)
        {
            roll_list.Add(random.Next(1, 7));
        }
        Console.WriteLine("Dice rolled: " + string.Join(", ", roll_list));
        return roll_list;
    }

    public static int CountResult(List<int> rollList)
    {
        int result = 0;

        if (rollList.Distinct().Count() == 6) // Straight
        {
            result += 1200;
            Console.WriteLine("Result: " + result);
            return result;
        }
        else if (rollList.Count == 6 && rollList.GroupBy(x => x).All(g => g.Count() == 2)) // Three Pairs
        {
            result += 800;
            Console.WriteLine("Result: " + result);
            return result;
        }

        for (int i = 1; i <= 6; i++)
        {
            int count = rollList.Where(x => x == i).ToList().Count;

            if (count == 6) // Six-of-a-kind
            {
                RemoveCountedNumbers(rollList, i, count);
                result += AddToResult(i, 8);
            }
            else if (count == 5) // Five-of-a-Kind
            {
                RemoveCountedNumbers(rollList, i, count);
                result += AddToResult(i, 4);
            }
            else if (count == 4) // Four-of-a-Kind
            {
                RemoveCountedNumbers(rollList, i, count);
                result += AddToResult(i, 2);
            }
            else if (count == 3) // Triple
            {
                RemoveCountedNumbers(rollList, i, count);
                result += AddToResult(i, 1);
            }
        }

        int count1 = rollList.Where(x => x == 1).ToList().Count;
        int count5 = rollList.Where(x => x == 5).ToList().Count;

        for (int x = 1; x <= count1; x++) // Single 1
        {
            result += 100;
        }

        for (int y = 1; y <= count5; y++) // Single 5
        {
            result += 50;
        }

        Console.WriteLine("Result: " + result);
        return result;
    }

    public static void RemoveCountedNumbers(List<int> rollList, int number, int count)
    {
        bool rollListUpdated;
        for (int x = 0; x < count; x++)
        {
            rollListUpdated = rollList.Remove(number);
        }
    }

    public static int AddToResult(int number, int multiplier)
    {
        if (number == 1)
        {
            return 1000 * multiplier;
        }
        else
        {
            return number * 100 * multiplier;
        }
    }
}
