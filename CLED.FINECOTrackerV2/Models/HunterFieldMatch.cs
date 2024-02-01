using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLED.FINECOTrackerV2.Models;

internal class HunterFieldMatch : Match
{
    public HunterFieldMatch()
    {
        this._maxScore = 6;
        this._bullseyeValue = [6];
        this._centerValue = [5, 6];
        this._maxNumberOfRounds = 24;
        this._fileName = "hf.json";
        this._centerLabel = "6+5";
        this._bullseyeLabel = "6";
        this._invertedBullseyeLabels = true;
    }
}
