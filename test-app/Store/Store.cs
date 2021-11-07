using System.Collections.Generic;
using test_app.Generated.Reactive;

namespace test_app.Store
{
    public class Store
    {
        public Store(DependencyManager dependencyManager)
        {
            Message = new ReactiveValue<string>(dependencyManager, "Click?");
            Label = new ReactiveGetter<string, string>(dependencyManager, Message, msg => $"{msg} :-)");
            ShowText = new ReactiveValue<bool>(dependencyManager);
            Hidden = new ReactiveValue<bool>(dependencyManager);
            ShowHideLabel = new ReactiveGetter<string, bool>(dependencyManager, ShowText, visible => visible ? "Hide" : "Show");
            List = new ReactiveCollection<List<string>, string>(dependencyManager, new List<string> { "A", "B", "C" });
        }

        public ReactiveValue<string> Message { get; }
        public ReactiveGetter<string, string> Label { get; }
        public ReactiveValue<bool> ShowText { get; }
        public ReactiveValue<bool> Hidden { get; }
        public ReactiveGetter<string, bool> ShowHideLabel { get; }
        public ReactiveCollection<List<string>, string> List { get; }
    }
}