using LoginMiddlewareExample.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


app.UseCustomeLoginMiddleware();

app.Run(async (HttpContext context) =>
{
    await context.Response.WriteAsync("Hello World");
});

app.Run();
