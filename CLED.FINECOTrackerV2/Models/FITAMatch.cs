using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CLED.FINECOTrackerV2.Models;

internal class FITAMatch : Match
{
    public FITAMatch()
    {
        this._isManualBullseye = true;
        this._bullseyeValue = [10];
        this._centerValue = [10];
        this._maxScore = 10;
        this._maxNumberOfRounds = 12;
        this._fileName = "fm.json";
        this._bullseyeLabel = "X";
        this._centerLabel = "10";
    }
}
