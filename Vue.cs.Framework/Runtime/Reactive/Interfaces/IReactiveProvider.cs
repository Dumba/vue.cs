namespace Vue.cs.Framework.Runtime.Reactive.Interfaces
{
    public interface IReactiveProvider<TValue> : IReactiveProvider
    {
        TValue Get(IReactiveConsumer<TValue> consumer);
    }

    public interface IReactiveProvider
    {
    }
}