using Spectre.Console;
using Spectre.Console.Json;
using SpectreApp;
using System.Text.Json.Nodes;

#region Markup data console writing
AnsiConsole.MarkupLine("[bold green]Hello, World![/]");
AnsiConsole.MarkupLine("[bold red]This is a test.[/]");
AnsiConsole.MarkupLine("[underline blue]This is a test.[/]");
AnsiConsole.MarkupLine("[italic yellow]This is a test.[/]");
AnsiConsole.MarkupLine("[slowblink]This is a test.[/]");
// Emojis
AnsiConsole.MarkupLine("[bold green]Hello, World! 🌍[/]");
AnsiConsole.MarkupLine("[bold red]This is a test. 🚀[/]");
AnsiConsole.MarkupLine("[underline blue]This is a test. 🔵[/]");
AnsiConsole.MarkupLine("[italic yellow]This is a test. 💛[/]");
#endregion

#region Table data console writing
// Create a table
var table = new Table();
table.AddColumn("Column 1");
table.AddColumn("Column 2");
table.AddRow("Row 1, Cell 1", "Row 1, Cell 2");
table.AddRow("Row 2, Cell 1", "Row 2, Cell 2");
AnsiConsole.Write(table);
#endregion

//Console.ReadLine();
AnsiConsole.Clear();

#region FetchApiData and display in JsonText
// Fetch API data example JsonPlaceholder
string apiUrl = "https://jsonplaceholder.typicode.com/posts/1";
string data = await Helpers.FetchApiData(apiUrl);
JsonText json = new JsonText(data);
json.StringColor(Color.Yellow);
json.ColonColor(Color.Red);

AnsiConsole.Write(new Panel(json)
                    .Header("Api Response")
                    .Collapse()
                    .BorderColor(Color.White)
                    );
#endregion

AnsiConsole.Clear();
#region Fegilet Text
AnsiConsole.Write(new FigletText("Hello, Mani")
    .LeftJustified()
    .Color(Color.Red));

FigletText figletText = new FigletText("Maran!");
figletText.Justification = Justify.Center;
figletText.Color = Color.Yellow;
AnsiConsole.Write(figletText);
#endregion

#region Multiselect
List<string> usualNames = [
    "Foo Charis",
    "Bar Alex",
    "Baz Ben",
    "Qux Pege",
    "Quu Lubo"
    ];

List<string> familyNames = [
    "Charis",
    "Alex",
    "Ben"
    ];

List<string> favoriteNames = AnsiConsole.Prompt(
    new MultiSelectionPrompt<string>()
    .Title("Which are your favorite placeholder names?")
    .InstructionsText("Press <space> to toggle, <enter> to accept")
    //.AddChoices(usualNames)
    .AddChoiceGroup("Usual Names", usualNames)
    .AddChoiceGroup("Family Names", familyNames)
    );

foreach(string name in favoriteNames)
{
    AnsiConsole.MarkupLine($"[Green]{name}[/]");
}

#endregion

Console.Read();