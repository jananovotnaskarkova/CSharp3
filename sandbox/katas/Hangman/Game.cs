namespace Hangman;

using System.Globalization;
using System.Text.RegularExpressions;

public class Game
{
    private const int INITIAL_LIVES = 10;
    private readonly string? word;
    private readonly List<char>? hiddenWord;
    private int numberOfLives;
    public Game(string? customWord = null)
    {
        word = customWord ?? SelectNewWord(listOfWords);
        hiddenWord = [.. word.ToCharArray().Select(c => '*')];
        numberOfLives = INITIAL_LIVES;
    }
    private readonly List<string> listOfWords = ["KVETINA", "PRIRODA", "BYDLENI"];

    private static string SelectNewWord(List<string> list)
    {
        var r = new Random();
        return list[r.Next(0, list.Count)];
    }

    public void Play()
    {
        Console.WriteLine("Vitej ve hre Hangman!");
        Console.WriteLine("-------------------------------");
        Console.WriteLine($"Slovo ma {word.Length} pismen");
        Console.WriteLine(string.Join("", hiddenWord));
        Console.WriteLine($"Mas {numberOfLives} zivotu");
        Console.WriteLine("-------------------------------");

        Guess(word, hiddenWord, numberOfLives);
    }

    public void Guess(string word, List<char> hiddenWord, int numberOfLives)
    {
        bool gameInProgress = true;
        string? letter;
        string? upperLetter;
        List<string> guessedLettersIncorrect = [];
        List<string> guessedLettersCorrect = [];
        int number = numberOfLives;


        while (gameInProgress)
        {
            Console.WriteLine("Zadej pismeno:");

            // Read input, check if valid and not guessed before, otherwise ask for new input
            letter = GetInput(guessedLettersCorrect, guessedLettersIncorrect);

            // Convert to upper case
            upperLetter = letter.ToUpper(CultureInfo.CurrentCulture);

            // Process the letter, check if in word, update hidden word, if not in word, decrease lifes
            numberOfLives = ProcessLetter(upperLetter, guessedLettersCorrect, guessedLettersIncorrect);

            Console.WriteLine("-------------------------------");
            Console.WriteLine(string.Join("", hiddenWord));

            // Print previous guesses
            if (guessedLettersIncorrect.Count > 0)
            {
                Console.WriteLine("Predchozi neuspesne pokusy: " + string.Join(", ", guessedLettersIncorrect));
            }

            // Print number of lifes
            Console.WriteLine("Pocet zivotu: " + numberOfLives);

            // Check win/loss conditions
            if (!hiddenWord.Contains('*'))
            {
                Console.WriteLine("Vyhral jsi!");
                gameInProgress = false;
            }
            else if (numberOfLives == 0)
            {
                Console.WriteLine($"Prohral jsi, slovo bylo {word}");
                gameInProgress = false;
            }
            Console.WriteLine("-------------------------------");
        }
    }

    public string GetInput(List<string> guessedLettersCorrect, List<string> guessedLettersIncorrect)
    {
        string letter = "";
        bool correct_input = false;

        while (!correct_input)
        {
            letter = Console.ReadLine() ?? string.Empty;

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

    public int ProcessLetter(string upperLetter, List<string> guessedLettersCorrect, List<string> guessedLettersIncorrect)
    {
        if (word.Contains(upperLetter))
        {
            int index = word.IndexOf(upperLetter, StringComparison.Ordinal);
            while (index != -1)
            {
                hiddenWord[index] = upperLetter.ToCharArray()[0];
                index = word.IndexOf(upperLetter, index + 1, StringComparison.Ordinal);
            }
            guessedLettersCorrect.Add(upperLetter);
            Console.WriteLine($"Spravne, slovo obsahuje pismeno {upperLetter}");
        }
        else
        {
            numberOfLives--;
            guessedLettersIncorrect.Add(upperLetter);
            Console.WriteLine($"Spatne, zadane pismeno neni ve slove");
        }
        return numberOfLives;
    }
}
