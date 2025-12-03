using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MedizID.Web;
using MedizID.UI.Services;
using MedizID.UI.Services.Generated;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Microsoft.Kiota.Serialization.Json;
using Blazored.LocalStorage;
using AntDesign;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register local storage
builder.Services.AddBlazoredLocalStorage();

// Register AntDesign
builder.Services.AddAntDesign();

// Register Kiota API client
builder.Services.AddScoped<IAuthenticationProvider>(sp =>
{
    var localStorage = sp.GetRequiredService<ILocalStorageService>();
    return new BearerTokenAuthenticationProvider(localStorage);
});

builder.Services.AddScoped(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    
    // Determine API URL based on environment
    // In development: use localhost:5053
    // In production: use relative path /api (will use current host from browser)
    string apiUrl = builder.HostEnvironment.IsDevelopment()
        ? "http://localhost:5053"
        : $"{builder.HostEnvironment.BaseAddress}api";
    
    httpClient.BaseAddress = new Uri(apiUrl);
    
    var adapter = new HttpClientRequestAdapter(
        sp.GetRequiredService<IAuthenticationProvider>(),
        parseNodeFactory: new JsonParseNodeFactory(),
        serializationWriterFactory: new JsonSerializationWriterFactory(),
        httpClient: httpClient
    );
    
    return new MedizIdApiClient(adapter);
});

builder.Services.AddScoped<FacilityLayoutState>();
builder.Services.AddScoped<AppointmentLayoutState>();
builder.Services.AddScoped<DashboardService>();

await builder.Build().RunAsync();
