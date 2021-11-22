namespace test_app.Runtime.Reactive.Interfaces
{
    public interface IReactiveProvider<TValue> : IReactiveProvider
    {
        TValue Get();
    }

    public interface IReactiveProvider
    {
    }
}