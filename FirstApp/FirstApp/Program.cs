using Microsoft.Extensions.Primitives;
using System.Net.Http.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build(); // get a instance of WebApplication

app.MapGet("/", () => "Hello World!");

app.MapGet("/hello", () =>
{
    return "Hello";
});

//app.Run();


app.Run(async (HttpContext context) =>
{
    //context.Response.StatusCode = 400; change the response status code
    //context.Response.Headers["Content-Type"] = "text/html"; // change the headers information
    //context.Response.Headers["MyKey"] = "My Values";

    //await context.Response.WriteAsync("Hello");
    //await context.Response.WriteAsync("World");

    // await context.Response.WriteAsync(JsonContent.SerializeObject(new { Name = "John Doe", Age = 30 })); // return JSON data

    //await context.Response.WriteAsync("<h1> Hello World </h1>");
    //await context.Response.WriteAsync("<p> Hello World ASP.NET Core");



    // ----- Request ----- 
    //string path = context.Request.Path;
    //context.Response.Headers["content-type"] = "text/html";
    //await context.Response.WriteAsync($"<h1>Path :  </h1> {path}");

    // --- Query Parameters ---
    //if (context.Request.Method == "GET")
    //{
    //    if (context.Request.Query.ContainsKey("id"))
    //    {
    //        string id = context.Request.Query["id"]
    //        await context.Response.WriteAsync($"<h3> {id}</h3>");
    //    }
    //}

    // read all the query parameters
    //if(context.Request.Method == "GET")
    //{
    //    foreach (var queryParam in context.Request.Query)
    //    {
    //        await context.Response.WriteAsync($"<p> Query Parameter: {queryParam.Key} - {queryParam.Value}</p>");
    //    }
    //}
    //Console.WriteLine(context.Request.Headers["User-Agent"]);


    // post request read body data
    if (context.Request.Method == "POST")
    {
        //StreamReader reader = new StreamReader(context.Request.Body);
        //string bodyData = await reader.ReadToEndAsync();
        //await context.Response.WriteAsync($"<h3> Body: {bodyData}</h3>");

        // -------- Both are same -----

        string body = await new StreamReader(context.Request.Body).ReadToEndAsync();
        await context.Response.WriteAsync($"<h3> Body -: : {body}</h3>");

        // parsing the Query data into dictionary with the
        // stringValues -> multiple value with same key
        // instead of stingVlues , string is only accept a single value of each key
        Dictionary<string, StringValues> queryData =  Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);

        foreach(var key in queryData)
        {
            await context.Response.WriteAsync($"<p> {key.Key} - {string.Join(", ", key.Value)}</p>");
        }
    }
    Console.WriteLine("Hello ji");
});

app.Run();

// To run the application, execute the following command in the terminal: