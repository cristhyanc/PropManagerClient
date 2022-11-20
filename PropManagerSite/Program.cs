using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PropManagerSite;
using Syncfusion.Blazor;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NzQ4MTkyQDMyMzAyZTMzMmUzMEJsNm9nYmpPZDlNODJVK0ROdVdQRllNZ3dwWVdCN0cwcFFzclVQVlZMZWc9");
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMicrosoftGraphClient("https://graph.microsoft.com/User.Read");

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("https://graph.microsoft.com/User.Read");
});

builder.Services.AddSyncfusionBlazor();
builder.Services.AddPropManagerSiteQL().ConfigureHttpClient(client=> client.BaseAddress = new Uri("https://localhost:44368/graphql"));
await builder.Build().RunAsync();

