using System.Threading.Tasks;

namespace test_app.Runtime.Reactive.Interfaces
{
    public interface IReactiveConsumer<TValue> : IReactiveConsumer
    {
        ValueTask Changed(TValue oldValue, TValue newValue);
    }

    public interface IReactiveConsumer
    {
    }
}