namespace Middle_ware.Middleware
{
    public class Authentication : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Method == "GET")
            {
                if (context.Request.Query.ContainsKey("id") && context.Request.Query["id"] == "123")
                {
                    await context.Response.WriteAsync("<h1> You are Authorized</h1>");
                    await next(context);
                }
                else{ 
                    await context.Response.WriteAsync($"<p> You are not Authorized </p>");
                }
            }
        }
    }
}
