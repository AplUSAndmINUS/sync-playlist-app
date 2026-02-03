using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SyncPlaylistApp.Core;
using SyncPlaylistApp.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register Core services (auth, playlists, sync)
builder.Services.AddSyncPlaylistServices();

// Override MainViewModel registration for Web app - use Singleton to share state across pages
// (Core registers as Transient by default, which would create separate instances per page)
builder.Services.AddSingleton<SyncPlaylistApp.Core.ViewModels.MainViewModel>();

await builder.Build().RunAsync();
