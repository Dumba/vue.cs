using System;
using test_app.Runtime.Reactive.Interfaces;

namespace test_app.Runtime.Reactive
{
    public class ReactiveAttribute : IReactiveConsumer
    {
        public Guid PageItemId { get; }
    }
}