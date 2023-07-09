namespace ViewModels
{
    internal interface IEquivalence<T>
    {
        bool IsEquivalentTo(T other);
    }
}
