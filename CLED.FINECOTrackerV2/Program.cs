using CLED.FINECOTrackerV2.Models;
using Spectre.Console;

namespace CLED.FINECOTrackerV2;

internal class Program
{
    static void Main(string[] args)
    {
        var matchType = AnsiConsole.Prompt(
            new SelectionPrompt<Enums.MatchType>()
                .Title("What type of game do you want to play?")
                .AddChoices(new[] { Enums.MatchType.Fita, Enums.MatchType.Indoor, Enums.MatchType.HunterField })
        );

        Match match;

        switch (matchType)
        {
            case Enums.MatchType.Fita:
                match = new FITAMatch();
                break;
            case Enums.MatchType.Indoor:
                match = new IndoorMatch();
                break;
            case Enums.MatchType.HunterField:
                match = new HunterFieldMatch();
                break;
            default:
                match = new FITAMatch();
                break;
        }

        var action = AnsiConsole.Prompt(
            new SelectionPrompt<Enums.Action>()
        .Title("Select the desired action:")
        .AddChoices(new[] { Enums.Action.New, Enums.Action.Continue, Enums.Action.PrintScore })
        );

        switch (action)
        {
            case Enums.Action.New:
                match.Archer = new Archer();
                match.Archer.Name = AnsiConsole.Ask<string>("Input the [green]name[/] of the archer:");
                match.Archer.Team = AnsiConsole.Ask<string>("Input the [green]team[/] of the archer:");
                match.Date = DateTime.Now;
                match.Scores = new List<Score>();
                match.SetScores(0);
                match.SaveScore();
                AnsiConsole.Markup($"The match created on {match.Date.ToString("dd/MM/yyyy hh:mm")} has been saved with a total of {match.Scores.Count} rounds");
                match.PrintScore();
                break;
            case Enums.Action.Continue:
                if (!match.LoadScore()) return;
                match.SaveScore();
                AnsiConsole.Markup($"The match created on {match.Date.ToString("dd/MM/yyyy hh:mm")} has been saved with a total of {match.Scores.Count} rounds");
                match.PrintScore();
                break;
            case Enums.Action.PrintScore:
                match.PrintScore();
                break;
            default:
                break;
        }
    }
}
