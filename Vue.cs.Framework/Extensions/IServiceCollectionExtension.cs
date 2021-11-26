using Microsoft.Extensions.DependencyInjection;

namespace Vue.cs.Framework.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static void AddVueCs(this IServiceCollection self)
        {
            self.AddScoped<Runtime.Reactive.JsManipulator>();
            self.AddScoped<Runtime.Reactive.DependencyManager>();

            self.AddTransient(typeof(Runtime.Reactive.Data.ReactiveValueGetter<,>.Builder));
            self.AddTransient(typeof(Runtime.Reactive.PageItems.ReactivePageMultiItem<>.Builder));
            self.AddTransient<Runtime.Reactive.PageItems.ReactiveText.Builder>();
            self.AddTransient<Runtime.Reactive.PageItems.ReactiveAttribute.Builder>();
            self.AddTransient<Runtime.Reactive.PageItems.ReactivePageItem.Builder>();
        }
    }
}