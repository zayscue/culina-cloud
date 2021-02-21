using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

WebHost.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {

    })
    .Configure((WebHostBuilderContext webHost, IApplicationBuilder app) =>
    {
        var env = webHost.HostingEnvironment;
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        });
    })
    .Build()
    .Run();
