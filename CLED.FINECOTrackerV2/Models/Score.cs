using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLED.FINECOTrackerV2.Models
{
    public class Score
    {
        public int Arrow1 { get; set; }
        public int Arrow2 { get; set; }
        public int Arrow3 { get; set; }
        public int Partial { get => Arrow1 + Arrow2 + Arrow3; }
        public int Center { get; set; }
        public int Bullseye { get; set; }
    }
}
