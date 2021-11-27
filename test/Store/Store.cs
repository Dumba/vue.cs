using System;
using System.Collections.Generic;
using Vue.cs.Framework.Runtime.Reactive;
using Vue.cs.Framework.Runtime.Reactive.Data;

namespace test.Store
{
    public class Store
    {
        public Store(DependencyManager dependencyManager)
        {
            Message = new ReactiveValue<string?>(dependencyManager, "Click?");
            Label = new ReactiveValueGetter<string?, string?>(dependencyManager, msg => $"{msg} :-)", Message);
            ShowText = new ReactiveValue<bool>(dependencyManager, true);
            Hidden = new ReactiveValue<bool>(dependencyManager);
            ShowHideLabel = new ReactiveValueGetter<bool, string?>(dependencyManager, visible => visible ? "Hide" : "Show", ShowText);
            List = new ReactiveCollection<string>(dependencyManager, new List<string> { "A", "B", "C" });
        }

        public ReactiveValue<string?> Message { get; }
        public ReactiveValueGetter<string?, string?> Label { get; }
        public ReactiveValue<bool> ShowText { get; }
        public ReactiveValue<bool> Hidden { get; }
        public ReactiveValueGetter<bool, string?> ShowHideLabel { get; }
        public ReactiveCollection<string> List { get; }
    }
}