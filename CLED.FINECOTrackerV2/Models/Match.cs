using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CLED.FINECOTrackerV2.Models;

public abstract class Match
{
    protected int _maxScore { get; set; }
    protected IEnumerable<int> _bullseyeValue { get; set; } = [10];
    protected IEnumerable<int> _centerValue { get; set; } = [10];
    protected int _maxNumberOfRounds { get; set; }
    protected string _fileName { get; set; } = string.Empty;
    protected bool _isManualBullseye { get; set; } = false;
    protected bool _invertedBullseyeLabels { get; set; } = false;
    protected string _bullseyeLabel { get; set; } = string.Empty;
    protected string _centerLabel { get; set; } = string.Empty;
    public Archer Archer { get; set; }
    public DateTime Date { get; set; }
    public List<Score> Scores { get; set; }
    public virtual void PrintScore()
    {
        if (!File.Exists(_fileName))
        {
            AnsiConsole.MarkupLine("[red]There is not a saved game[/]");
            AnsiConsole.MarkupLine("Program will close after pressing a button");
            Console.ReadLine();
            return;
        }

        var savedGame = JsonSerializer.Deserialize<SaveView>(File.ReadAllText(_fileName));
        this.Archer = savedGame.Archer;
        this.Date = savedGame.Date;
        this.Scores = savedGame.Scores;

        var t = new Table()
            .Border(TableBorder.HeavyHead)
            .AddColumn("")
            .AddColumn(new TableColumn("1").Centered())
            .AddColumn(new TableColumn("2").Centered())
            .AddColumn(new TableColumn("3").Centered())
            .AddColumn(new TableColumn("Prog.").Centered())
            .AddColumn(new TableColumn("Tot.").Centered());
        if (_invertedBullseyeLabels)
        {
            t.AddColumn(new TableColumn(_bullseyeLabel).Centered())
                .AddColumn(new TableColumn(_centerLabel).Centered());
        }
        else
        {
            t.AddColumn(new TableColumn(_centerLabel).Centered())
                .AddColumn(new TableColumn(_bullseyeLabel).Centered());
        }

        int counter = 0;
        int rowTotal = 0;
        foreach (var item in this.Scores)
        {
            counter++;
            rowTotal += item.Partial;
            t.AddRow(
                counter.ToString(),
                item.Arrow1.ToString(),
                item.Arrow2.ToString(),
                item.Arrow3.ToString(),
                item.Partial.ToString(),
                rowTotal.ToString(),
                _invertedBullseyeLabels ? item.Bullseye.ToString() : item.Center.ToString(),
                _invertedBullseyeLabels ? item.Center.ToString() : item.Bullseye.ToString());
        }

        if (_invertedBullseyeLabels)
            t.AddRow(string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                "[green][bold]TOTAL[/][/]",
                $"[gray]{rowTotal}[/]",
                "[gray]" + this.Scores.Sum(x => x.Bullseye) + "[/]",
                "[gray]" + this.Scores.Sum(x => x.Center) + "[/]");
        else
            t.AddRow(string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                "[green][bold]TOTAL[/][/]",
                $"[gray]{rowTotal}[/]",
                "[gray]" + this.Scores.Sum(x => x.Center) + "[/]",
                "[gray]" + this.Scores.Sum(x => x.Bullseye) + "[/]");

        AnsiConsole.WriteLine();
        AnsiConsole.Write(t);
    }
    public virtual void SetScores(int indexToStart)
    {
        for (int i = indexToStart; i < _maxNumberOfRounds; i++)
        {
            var s = new Score();
            s.Arrow1 = PromptScore(1, i + 1);
            s.Arrow2 = PromptScore(2, i + 1);
            s.Arrow3 = PromptScore(3, i + 1);

            s.Center = 0;
            if (_centerValue.Contains(s.Arrow1)) s.Center++;
            if (_centerValue.Contains(s.Arrow2)) s.Center++;
            if (_centerValue.Contains(s.Arrow3)) s.Center++;

            if (s.Center > 0 && _isManualBullseye)
            {
                var sp = new SelectionPrompt<int>().Title("How many Xs have been hit?");
                switch (s.Center)
                {
                    case 1:
                        sp.AddChoice(0);
                        sp.AddChoice(1);
                        break;
                    case 2:
                        sp.AddChoice(0);
                        sp.AddChoice(1);
                        sp.AddChoice(2);
                        break;
                    case 3:
                        sp.AddChoice(0);
                        sp.AddChoice(1);
                        sp.AddChoice(2);
                        sp.AddChoice(3);
                        break;
                    default:
                        break;
                }
                s.Bullseye = AnsiConsole.Prompt(sp);
            }
            else
            {
                s.Bullseye = 0;
                if (_bullseyeValue.Contains(s.Arrow1)) s.Bullseye++;
                if (_bullseyeValue.Contains(s.Arrow2)) s.Bullseye++;
                if (_bullseyeValue.Contains(s.Arrow3)) s.Bullseye++;
            }
            Scores.Add(s);
            if (i == _maxNumberOfRounds) break;
            if (!AnsiConsole.Confirm("Do you want to continue inserting scores?")) break;
        }
    }
    public virtual bool LoadScore()
    {
        if (!File.Exists(_fileName))
        {
            if (!AnsiConsole.Confirm("There currently [red]is not[/] a saved game. Would you like to start a [yellow]new[/] one?"))
                return false;
            this.Scores = new List<Score>();
            SetScores(0);
            return true;
        }
        else
        {
            AnsiConsole.Progress()
                .Start(ctx =>
                {
                    var task1 = ctx.AddTask("Loading the [green]match file[/]");

                    while (!ctx.IsFinished)
                    {
                        task1.Increment(1.5);
                        Thread.Sleep(50);
                    }
                });
            var savedGame = JsonSerializer.Deserialize<SaveView>(File.ReadAllText(_fileName));
            if (savedGame.Scores.Count == _maxNumberOfRounds)
            {

                AnsiConsole.MarkupLine("[yellow]The match has reached its maximum amount of rounds . . . [/]");
                return false;
            }
            this.Archer = savedGame.Archer;
            this.Scores = savedGame.Scores;
            this.Date = savedGame.Date;
            SetScores(this.Scores.Count);
            return true;
        }
    }
    public virtual void SaveScore()
    {
        File.WriteAllText(_fileName, JsonSerializer.Serialize(new SaveView(this.Date, this.Archer, this.Scores)));
    }
    private int PromptScore(int arrowNumber, int round)
    {
        string s = string.Empty;
        switch (arrowNumber)
        {
            case 1:
                s = "first";
                break;
            case 2:
                s = "second";
                break;
            case 3:
                s = "third";
                break;
            default:
                break;
        }
        return AnsiConsole.Prompt(
                    new TextPrompt<int>($"Input the [green]{s} arrow score[/] for the [yellow]{round}o[/] round:")
                        .ValidationErrorMessage($"Range must be between 0-{_maxScore}")
                        .Validate(s =>
                        {
                            if (s > _maxScore) return ValidationResult.Error($"[red]Number must be lesser than or equal of {_maxScore}[/]");
                            if (s < 0) return ValidationResult.Error("[red]Number must be greater or equal than 0[/]");
                            return ValidationResult.Success();
                        })
                );
    }
}
