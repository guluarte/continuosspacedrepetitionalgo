using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContinuosSpacedRepetitionAlgo
{
    class Item
    {
        public string Video { get; set; }
        public double Interval { get; set; }
        public DateTime LastView { get; set; }

        public int Views { get; set; }
    }
}
