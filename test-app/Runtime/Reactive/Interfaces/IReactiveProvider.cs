namespace test_app.Runtime.Reactive.Interfaces
{
    public interface IReactiveProvider<TValue> : IReactiveProvider
    {
        TValue Value { get; }
    }

    public interface IReactiveProvider
    {
    }
}