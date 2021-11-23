using System;
using Microsoft.Extensions.DependencyInjection;
using test_app.Runtime.Reactive;
using test_app.Runtime.Reactive.Data;

namespace test_app.Store
{
    public class Store
    {
        public Store(IServiceProvider serviceProvider, DependencyManager dependencyManager)
        {
            Message = new ReactiveValue<string>(dependencyManager, "Click?");
            Label = serviceProvider.GetService<ReactiveValueGetter<string, string>.Builder>()
                .Build(msg => $"{msg} :-)", Message);
            ShowText = new ReactiveValue<bool>(dependencyManager, true);
            Hidden = new ReactiveValue<bool>(dependencyManager);
            ShowHideLabel = serviceProvider.GetService<ReactiveValueGetter<bool, string>.Builder>()
                .Build(visible => visible ? "Hide" : "Show", ShowText);
            // List = new ReactiveList<string>(dependencyManager, new List<string> { "A", "B", "C" });
        }

        public ReactiveValue<string> Message { get; }
        public ReactiveValueGetter<string, string> Label { get; }
        public ReactiveValue<bool> ShowText { get; }
        public ReactiveValue<bool> Hidden { get; }
        public ReactiveValueGetter<bool, string> ShowHideLabel { get; }
        // public ReactiveList<string> List { get; }
    }
}