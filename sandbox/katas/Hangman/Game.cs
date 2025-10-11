namespace Hangman;

using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

public class Game
{
    private static string word;

    private static List<char> hiddenWord;
    private static int numberOfLifes = 10;
    private static bool gameInProgress = false;
    private static List<string> guessedLettersIncorrect = new List<string>();
    private static List<string> guessedLettersCorrect = new List<string>();

    // public Game(string word, int numberOfLifes, bool gameInProgress)
    // {
    //     Word = word;
    //     NumberOfLifes = numberOfLifes;
    //     GameInProgress = gameInProgress;
    // }

    private static List<string> listOfWords = new List<string> { "KVETINA", "PRIRODA", "BYDLENI" };

    private static string SelectNewWord()
    {
        var r = new Random();
        return listOfWords[r.Next(0, listOfWords.Count)];
    }

    public static void StartGame()
    {
        word = SelectNewWord();
        hiddenWord = word.ToCharArray().Select(c => '*').ToList();
        Console.WriteLine("Vitej ve hre Hangman!");
        Console.WriteLine("-------------------------------");
        Console.WriteLine($"Slovo ma {word.Length} pismen");
        Console.WriteLine(string.Join("", hiddenWord));
        Console.WriteLine($"Mas {numberOfLifes} zivotu");
        Console.WriteLine("-------------------------------");
        gameInProgress = true;
        Guess();
    }

    public static void Guess()
    {
        while (gameInProgress)
        {
            Console.WriteLine("Zadej pismeno:");

            // Read input, check if valid and not guessed before, otherwise ask for new input
            string letter = GetInput();

            // Convert to upper case
            string upperLetter = letter.ToUpper(CultureInfo.CurrentCulture);

            // Process the letter, check if in word, update hidden word, if not in word, decrease lifes
            ProcessLetter(upperLetter);

            Console.WriteLine("-------------------------------");
            Console.WriteLine(string.Join("", hiddenWord));

            // Print previous guesses
            if (guessedLettersIncorrect.Count > 0)
            {
                Console.WriteLine("Predchozi pokusy: " + string.Join(", ", guessedLettersIncorrect));
            }

            // Print number of lifes
            Console.WriteLine("Pocet zivotu: " + numberOfLifes);

            // Check win/loss conditions
            if (!hiddenWord.Contains('*'))
            {
                Console.WriteLine("Vyhral jsi!");
                gameInProgress = false;
            }
            else if (numberOfLifes == 0)
            {
                Console.WriteLine($"Prohral jsi, slovo bylo {word}");
                gameInProgress = false;
            }
            Console.WriteLine("-------------------------------");
        }
    }

    public static string GetInput()
    {
        string letter = "";
        bool correct_input = false;

        while (!correct_input)
        {
            letter = Console.ReadLine();

            if (Regex.IsMatch(letter, @"^[a-zA-Z]+$") && letter.Length == 1)
            {
                if (guessedLettersCorrect.Contains(letter.ToUpper(CultureInfo.CurrentCulture))
                    || guessedLettersIncorrect.Contains(letter.ToUpper(CultureInfo.CurrentCulture)))
                {
                    Console.WriteLine("Toto pismeno jsi uz hadal, zkus to znovu:");
                    continue;
                }
                correct_input = true;
            }
            else
            {
                Console.WriteLine("Neplatny vstup, zkus to znovu:");
            }
        }
        return letter;
    }

    public static void ProcessLetter(string upperLetter)
    {
        if (word.Contains(upperLetter))
        {
            int index = word.IndexOf(upperLetter);
            while (index != -1)
            {
                hiddenWord[index] = upperLetter.ToCharArray()[0];
                index = word.IndexOf(upperLetter, index + 1);
            }
            guessedLettersCorrect.Add(upperLetter);
            Console.WriteLine($"Spravne, slovo obsahuje pismeno {upperLetter}");
        }
        else
        {
            numberOfLifes--;
            guessedLettersIncorrect.Add(upperLetter);
            Console.WriteLine($"Spatne, zadane pismeno neni ve slove");
        }
    }
}


