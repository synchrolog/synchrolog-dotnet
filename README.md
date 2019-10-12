# synchrolog-dotnet
Synchrolog library for ASP.NET Core

At Startup class, add Synchrolog middleware as first in the request pipeline. Then add the Synchrolog provider passing the application builder as argument:

```
// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
{
    app.UseSynchrolog();

    loggerFactory.AddSynchrologProvider(app);
    ...
```

Add Synchrolog service at Startup class by replacing ACCESS_TOKEN by your API key:

```
// This method gets called by the runtime. Use this method to add services to the container.
public void ConfigureServices(IServiceCollection services)
{
    services.AddSynchrolog("ACCESS_TOKEN");
    ...
```
