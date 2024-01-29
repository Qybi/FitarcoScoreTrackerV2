using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLED.FINECOTrackerV2.Models
{
    public class SaveView
    {
        public DateTime Date { get; set; }
        public Archer Archer { get; set; }
        public List<Score> Scores { get; set; }
        public SaveView(DateTime date, Archer archer, List<Score> scores)
        {
            Date = date;
            Archer = archer;
            Scores = scores;
        }

    }
}
