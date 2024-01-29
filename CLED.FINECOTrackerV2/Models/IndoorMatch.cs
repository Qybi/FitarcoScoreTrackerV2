using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLED.FINECOTrackerV2.Models
{
    internal class IndoorMatch : Match
    {
        public IndoorMatch()
        {
            this._invertedBullseyeLabels = true;
            this._maxScore = 10;
            this._bullseyeValue = [10];
            this._centerValue = [9];
            this._maxNumberOfRounds = 10;
            this._fileName = "in.json";
            this._bullseyeLabel = "10";
            this._centerLabel = "9";
        }
    }
}
