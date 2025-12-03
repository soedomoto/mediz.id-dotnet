using AntDesign;
using Blazored.LocalStorage;
using MedizID.UI.Services;
using MedizID.UI.Services.Generated;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Microsoft.Kiota.Serialization.Json;
using Microsoft.Extensions.Logging;

namespace MedizID.MAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
		builder.Services.AddBlazoredLocalStorage();
		builder.Services.AddAntDesign();

		// Register HttpClient
		builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://api.mediz-id.local/") });

		// Register Kiota API client
		builder.Services.AddScoped<IAuthenticationProvider>(sp =>
		{
			var localStorage = sp.GetRequiredService<ILocalStorageService>();
			return new BearerTokenAuthenticationProvider(localStorage);
		});

		builder.Services.AddScoped(sp =>
		{
			var httpClient = sp.GetRequiredService<HttpClient>();

			// In MAUI, we can configure the API URL here
			// For development, use localhost:5053
			// For production, update this URL accordingly
#if DEBUG
			httpClient.BaseAddress = new Uri("http://localhost:5053");
#else
			httpClient.BaseAddress = new Uri("http://mediz.soedomoto.com/api");
#endif
			
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

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
		builder.Logging.SetMinimumLevel(LogLevel.Debug);
#endif

		return builder.Build();
	}
}
