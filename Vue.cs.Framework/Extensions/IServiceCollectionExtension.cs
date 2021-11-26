using Microsoft.Extensions.DependencyInjection;

namespace Vue.cs.Framework.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static void AddVueCs(this IServiceCollection self)
        {
            self.AddScoped<Runtime.Reactive.JsManipulator>();
            self.AddScoped<Runtime.Reactive.DependencyManager>();
        }
    }
}