# synchrolog-dotnet
Synchrolog library for ASP.NET Core

Add Synchrolog middleware at Startup class and make it the first one to load:

```
// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseSynchrolog();
    ...
```

Add Synchrolog service at Startup class replacing ACCESS_TOKEN by your API key:

```
// This method gets called by the runtime. Use this method to add services to the container.
public void ConfigureServices(IServiceCollection services)
{
    services.AddSynchrolog("ACCESS_TOKEN");
	...
```
