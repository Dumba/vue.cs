using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Vue.cs.Framework.Runtime.Reactive;
using Vue.cs.Framework.Runtime.Reactive.Data;

namespace test.Store
{
    public class Store
    {
        public Store(IServiceProvider serviceProvider, DependencyManager dependencyManager)
        {
            Message = new ReactiveValue<string>(dependencyManager, "Click?");
            Label = serviceProvider.GetService<ReactiveValueGetter<string, string>.Builder>()!
                .Build(msg => $"{msg} :-)", Message);
            ShowText = new ReactiveValue<bool>(dependencyManager, true);
            Hidden = new ReactiveValue<bool>(dependencyManager);
            ShowHideLabel = serviceProvider.GetService<ReactiveValueGetter<bool, string>.Builder>()!
                .Build(visible => visible ? "Hide" : "Show", ShowText);
            List = new ReactiveCollection<string>(dependencyManager, new List<string> { "A", "B", "C" });
        }

        public ReactiveValue<string> Message { get; }
        public ReactiveValueGetter<string, string> Label { get; }
        public ReactiveValue<bool> ShowText { get; }
        public ReactiveValue<bool> Hidden { get; }
        public ReactiveValueGetter<bool, string> ShowHideLabel { get; }
        public ReactiveCollection<string> List { get; }
    }
}