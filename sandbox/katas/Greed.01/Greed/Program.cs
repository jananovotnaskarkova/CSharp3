using Greed;

Random random = new();
int diceCount = random.Next(1, 7);

Console.WriteLine("First player's turn:");
var firstPlayer = new Player(Play.CountResult(Play.Roll(diceCount)));
Console.WriteLine("---------------------");
Console.WriteLine("Second player's turn:");
var secondPlayer = new Player(Play.CountResult(Play.Roll(diceCount)));
Console.WriteLine("---------------------");

if (firstPlayer.Result > secondPlayer.Result)
{
    Console.WriteLine("First player wins!");
}
else if (firstPlayer.Result < secondPlayer.Result)
{
    Console.WriteLine("Second player wins!");
}
else
{
    Console.WriteLine("It's a tie!");
}
