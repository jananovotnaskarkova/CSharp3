var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/pozdrav/{jmeno}", (string jmeno) => $"Ahoj {jmeno}!");
app.MapGet("/secti/{a}/{b}", (int a, int b) => $"Vysledek: {a} + {b} = {a + b}");

app.Run();
