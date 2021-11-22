using Microsoft.Extensions.DependencyInjection;

namespace test_app.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static void AddVueCs(this IServiceCollection self)
        {
            self.AddScoped<Runtime.Reactive.JsManipulator>();
            self.AddScoped<Runtime.Reactive.DependencyManager>();

            self.AddTransient(typeof(Runtime.Reactive.Data.ReactiveValueGetter<,>.Builder));
            self.AddTransient<Runtime.Reactive.PageItems.ReactiveText.Builder>();
            self.AddTransient<Runtime.Reactive.PageItems.ReactiveAttribute.Builder>();
            self.AddTransient<Runtime.Reactive.PageItems.ReactivePageItem.Builder>();
        }
    }
}