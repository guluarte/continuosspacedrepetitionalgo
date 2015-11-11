namespace ContinuosSpacedRepetition.Interface
{
    interface IContinuosSpacedRepetitionList<T>
    {
        void Add(T item);
        T GetNext();
        T GetFirst();
        T GetLast();
        T GetRandom();
        T GetRandomLearned();
        void Remove(T item);
        int Length();
    }
}