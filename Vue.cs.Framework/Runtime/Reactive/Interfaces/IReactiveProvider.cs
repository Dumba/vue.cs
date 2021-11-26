namespace Vue.cs.Framework.Runtime.Reactive.Interfaces
{
    public interface IReactiveProvider<TValue> : IReactiveProvider
    {
        TValue? Value { get; }
        void Register(IReactiveConsumer<TValue> consumer);
    }

    public interface IReactiveProvider
    {
    }
}