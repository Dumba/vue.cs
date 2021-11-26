using System.Threading.Tasks;

namespace Vue.cs.Framework.Runtime.Reactive.Interfaces
{
    public interface IReactiveConsumer<TValue> : IReactiveConsumer
    {
        ValueTask Changed(TValue oldValue, TValue newValue);
    }

    public interface IReactiveConsumer
    {
    }
}