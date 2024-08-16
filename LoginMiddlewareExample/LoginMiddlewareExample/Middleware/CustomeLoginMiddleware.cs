
using System.Text.Json;

namespace LoginMiddlewareExample.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CustomeLoginMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomeLoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if(httpContext.Request.Method == HttpMethods.Post && httpContext.Request.Path == "/login")
            {
                // Check if username and password are correct
                var body = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
                //httpContext.Request.Body.Position = 0;

                var loginRequest = JsonSerializer.Deserialize<LoginRequest>(body);

                if (loginRequest != null && loginRequest.Username == "admin" && loginRequest.Password == "admin123")
                {
                    httpContext.Response.StatusCode = 200;
                    await httpContext.Response.WriteAsync("Login Successful");
                }
                else
                {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Invalid Credentials");
                }

            }

            await _next(httpContext);
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomeLoginMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomeLoginMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomeLoginMiddleware>();
        }
    }
}
