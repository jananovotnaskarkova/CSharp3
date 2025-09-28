using Greed;

Console.WriteLine("First player's turn:");
Player firstPlayer = new Player(Play.CountResult(Play.Roll()));
Console.WriteLine("---------------------");
Console.WriteLine("Second player's turn:");
Player secondPlayer = new Player(Play.CountResult(Play.Roll()));
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
