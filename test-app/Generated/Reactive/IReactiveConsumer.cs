using System.Threading.Tasks;

namespace test_app.Generated.Reactive
{
    public interface IReactiveConsumer<TValue> : IReactiveConsumer
    {
        Task Changed(TValue oldValue, TValue newValue);
    }

    public interface IReactiveConsumer
    {
    }
}