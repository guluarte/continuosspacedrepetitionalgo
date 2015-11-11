using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContinuosSpacedRepetition.Models
{
    public class ListComparer : IComparer
    {
        public int Compare(Card x, Card y)
        {
            if (x.GetScore() > y.GetScore())
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int Compare(object x, object y)
        {
            return Compare((Card)x, (Card)y);
        }
    }
}
