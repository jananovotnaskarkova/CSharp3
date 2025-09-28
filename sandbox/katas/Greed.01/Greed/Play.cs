namespace Greed;

public class Play(List<int> rollList, int result)
{
    private List<int> RollList = rollList;
    private int Result = result;

    public static List<int> Roll()
    {
        Random random = new();
        List<int> roll_list = [];

        for (int i = 0; i < 5; i++)
        {
            roll_list.Add(random.Next(1, 7));
        }
        Console.WriteLine("Dice rolled: " + string.Join(", ", roll_list));
        return roll_list;
    }

    public static int CountResult(List<int> rollList)
    {
        int result = 0;

        for (int i = 1; i <= 6; i++)
        {
            int count = rollList.Where(x => x == i).ToList().Count;

            if (count >= 3)
            {
                result += i * 100;

                for (int j = 0; j < 3; j++)
                {
                    var rollListUpdated = rollList.Remove(i);
                }

                if (i == 1)
                {
                    result += 900;
                }
            }
        }

        int count1 = rollList.Where(x => x == 1).ToList().Count;
        int count5 = rollList.Where(x => x == 5).ToList().Count;

        for (int x = 1; x <= count1; x++)
        {
            result += 100;
        }

        for (int y = 1; y <= count5; y++)
        {
            result += 50;
        }

        Console.WriteLine("Result: " + result);
        return result;
    }
}
