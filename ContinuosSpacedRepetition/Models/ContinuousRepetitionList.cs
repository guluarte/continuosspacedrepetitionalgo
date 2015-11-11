using System.Collections;
using ContinuosSpacedRepetition.Interface;

namespace ContinuosSpacedRepetition.Models
{
    public class ContinuousRepetitionList : IContinuosSpacedRepetitionList<Card>
    {
        private readonly SortedList _list;

        public ContinuousRepetitionList()
        {
            _list = new SortedList(new ListComparer());
        }

        public void Add(Card item)
        {
           _list.Add(item, 0);
        }

        public Card GetNext()
        {
            return GetFirst();
        }

        public Card GetFirst()
        {
            return (Card)_list[0];
        }

        public Card GetLast()
        {
            return (Card)_list[_list.Count];
        }

        public Card GetRandom()
        {
            throw new System.NotImplementedException();
        }

        public Card GetRandomLearned()
        {
            throw new System.NotImplementedException();
        }

        public void Remove(Card item)
        {
            _list.Remove(item);
        }

        public int Length()
        {
            return _list.Count;
        }
    }
}