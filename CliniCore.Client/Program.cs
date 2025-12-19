using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CliniCore.Client;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// LEER URL DE LA API
var apiUrl = builder.Configuration.GetValue<string>("ApiBaseUrl") ?? "http://localhost:5272";

// CONFIGURAR HTTP CLIENT PARA QUE SIEMPRE APUNTE A LA API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });

// REGISTRAR EL SERVICIO
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
