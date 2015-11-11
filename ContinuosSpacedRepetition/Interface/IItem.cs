using System;

namespace ContinuosSpacedRepetition.Interface
{
    public interface IItem<K, T>
    {
        Guid GetKey();
        T GetValue();
        void SetValue(T value);
        long GetScore();
        void SetScore(long score);
        DateTime GetLastView();
        void SetLastView(DateTime lastViewDateTime);
    }
}