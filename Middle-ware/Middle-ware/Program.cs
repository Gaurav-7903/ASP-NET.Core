using Middle_ware.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<MyCustomMiddleware>(); // attached the class middle ware (DI)
//builder.Services.AddTransient<Authentication>();

var app = builder.Build();


// middle ware 1 
app.Use(async (HttpContext context, RequestDelegate next) => {
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync("<h3>Hello World 1</h3>"); // 1 (Execution Sequence)
    await next(context); // forward the request to subsequent middle ware
    await context.Response.WriteAsync("<h3>Hello World 11</h3>"); // 5
});

app.UseMiddleware<Authentication>();



// authentication

// middle ware 2 (next passing to next middle)
app.Use(async (HttpContext context, RequestDelegate next) => {
    await context.Response.WriteAsync("<h3>Hello World 2</h3>"); // 2
    await next(context);
    await context.Response.WriteAsync("<h3>Hello World 22</h3>"); // 4
});

// calling a custom middle ware
//app.UseMiddleware<MyCustomMiddleware>();  // calling custom middleware 
app.UseMyCustomMiddleware(); // instead of  above we can use directly this

app.Run(async (HttpContext context) =>
{
    await context.Response.WriteAsync("<h3>Hello 1</h3>"); // 3    // here is not next so it is called a terminated middle ware
});

app.Run();


// Note: Middlewares are executed in the order they are added to your application.
