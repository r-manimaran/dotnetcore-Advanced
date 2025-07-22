using Spectre.Console;

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

// Create a table
var table = new Table();
table.AddColumn("Column 1");
table.AddColumn("Column 2");
table.AddRow("Row 1, Cell 1", "Row 1, Cell 2");
table.AddRow("Row 2, Cell 1", "Row 2, Cell 2");
AnsiConsole.Write(table);

Console.ReadLine();
AnsiConsole.Clear();