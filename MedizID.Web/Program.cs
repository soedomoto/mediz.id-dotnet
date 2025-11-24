using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MedizID.Web;
using MedizID.Web.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add Blazored Local Storage
builder.Services.AddBlazoredLocalStorage();

// Add MudBlazor Services
builder.Services.AddMudServices();

// Add Services
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<IApiService, ApiService>();

// Add HTTP Client with base address
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:8000/api/v1") });

// Add Logging
builder.Services.AddLogging();

await builder.Build().RunAsync();
