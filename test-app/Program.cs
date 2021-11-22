using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using test_app.Extensions;

namespace test_app
{
    public class Program
    {
        public static readonly string ParentSelector = "#app";
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>(ParentSelector);

            builder.Services.AddVueCs();

            // builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<Components.Master>();
            builder.Services.AddScoped<Components.Menu>();
            builder.Services.AddScoped<Store.Store>();

            await builder.Build().RunAsync();
        }
    }
}
