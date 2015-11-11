using System;
using ContinuosSpacedRepetition.Interface;

namespace ContinuosSpacedRepetition.Models
{
    public class Card : IItem<string, string>
    {
        private readonly Guid _key;
        private long _score;
        private DateTime _lastView;
        private string _value;

        public Card(string value)
        {
            _value = value;
            _key = Guid.NewGuid();
        }
        public Guid GetKey()
        {
            return _key;
        }

        public string GetValue()
        {
            return _value;
        }

        public void SetValue(string value)
        {
            _value = value;
        }

        public long GetScore()
        {
            return _score;
        }

        public void SetScore(long score)
        {
            _score = score;
        }

        public DateTime GetLastView()
        {
            return _lastView;
        }

        public void SetLastView(DateTime lastViewDateTime)
        {
            _lastView = lastViewDateTime;
        }

        public override int GetHashCode()
        {
            return _key.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return _key.Equals( ((Card)obj).GetKey() );
        }
    }
}