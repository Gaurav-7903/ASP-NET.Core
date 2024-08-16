using System.Runtime.CompilerServices;

namespace Middle_ware.Middleware
{
    public class MyCustomMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("<h4> Custom Middle ware Called : start </H4>");
            await next(context);
            await context.Response.WriteAsync("<h4> Custom Middle ware Called : end </H4>");
        }
    }


    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyCustomMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MyCustomMiddleware>();
        }
    }
}
