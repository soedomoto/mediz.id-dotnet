using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MedizID.Web;
using MedizID.Web.Services.Generated;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Microsoft.Kiota.Serialization.Json;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register local storage
builder.Services.AddBlazoredLocalStorage();

// Register Kiota API client
builder.Services.AddScoped<IAuthenticationProvider, AnonymousAuthenticationProvider>();
builder.Services.AddScoped(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    httpClient.BaseAddress = new Uri("http://localhost:5053");
    
    var adapter = new HttpClientRequestAdapter(
        sp.GetRequiredService<IAuthenticationProvider>(),
        parseNodeFactory: new JsonParseNodeFactory(),
        serializationWriterFactory: new JsonSerializationWriterFactory(),
        httpClient: httpClient
    );
    
    return new MedizIdApiClient(adapter);
});

await builder.Build().RunAsync();
