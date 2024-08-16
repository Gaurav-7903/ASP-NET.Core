var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWhen(
    context => context.Request.Query.ContainsKey("name"),
    app =>
    {
        app.Use(async (context , next) =>
        {
            await context.Response.WriteAsync($"Hello {context.Request.Query["name"]}");
            await next(context);
        });
    }
);

app.Run(async (context) =>
{
    await context.Response.WriteAsync("Hello From another Branch");
});


app.Run();
