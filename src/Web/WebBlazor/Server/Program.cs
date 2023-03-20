var builder = WebApplication.CreateBuilder(args);

builder.Configuration["BaseUrl"] = new Uri(builder.Configuration["ASPNETCORE_URLS"])?.LocalPath ?? "/";

RegisterAppInsights(builder);

builder.Services
    .AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddUrlGroup(new Uri(builder.Configuration["IdentityUrlHC"]), name: "identityapi-check", tags: new[] { "identityapi" });

builder.Services.Configure<AppSettings>(builder.Configuration);

if (builder.Configuration.GetValue<string>("IsClusterEnv") == bool.TrueString) {
    builder.Services
        .AddDataProtection(options => options.ApplicationDiscriminator = "eshop.webblazor")
        .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(builder.Configuration["DPConnectionString"]), "DataProtection-Keys");
}

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNameCaseInsensitive = true);

builder.Services.AddRazorPages();

builder.Host
    .UseContentRoot(Directory.GetCurrentDirectory())
    .ConfigureAppConfiguration((builderContext, config) => config.AddEnvironmentVariables())
    .ConfigureLogging((hostingContext, builder) => builder
        .AddConfiguration(hostingContext.Configuration.GetSection("Logging"))
        .AddConsole()
        .AddDebug()
        .AddAzureWebAppDiagnostics()
    )
    .UseSerilog((builderContext, config) => config
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .WriteTo.Seq("http://seq")
        .ReadFrom.Configuration(builderContext.Configuration)
        .WriteTo.Console()
    );

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
}
else {
    app.UseExceptionHandler("/Error");
}

WebContextSeed.Seed(app, app.Environment);

var pathBase = app.Configuration["PATH_BASE"];
if (!string.IsNullOrEmpty(pathBase)) {
    Log.Logger.Debug("Using PATH BASE '{pathBase}'", pathBase);
    app.UsePathBase(pathBase);
}

app.UseBlazorFrameworkFiles();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapDefaultControllerRoute();
app.MapFallbackToPage("/_Host");
app.MapHealthChecks("/liveness", new HealthCheckOptions {
    Predicate = r => r.Name.Contains("self")
});
app.MapHealthChecks("/hc", new HealthCheckOptions {
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

await app.RunAsync();

static void RegisterAppInsights(WebApplicationBuilder builder) =>
    builder.Services
        .AddApplicationInsightsTelemetry(builder.Configuration)
        .AddApplicationInsightsKubernetesEnricher();
