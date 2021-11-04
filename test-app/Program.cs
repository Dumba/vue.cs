using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using test_app.Generated;
using test_app.Generated.Reactive;

namespace test_app
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            // builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<JsManipulator>();
            builder.Services.AddScoped<DependencyManager>();

            builder.Services.AddScoped<Components.Master>();

            await builder.Build().RunAsync();
        }
    }
}
