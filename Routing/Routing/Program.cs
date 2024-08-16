using System.Runtime.InteropServices;
using Routing.CustomConstraint;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("SalesReportMonth", typeof(CustomSalesReportConstraint)); // adding a custom constraint
});

var app = builder.Build();

//app.Use(async (context, next) =>
//{
//    Endpoint enpoint = context.GetEndpoint();
//    Console.WriteLine("1" + enpoint);
//    await next(context);
//});

app.UseRouting(); // enable routing

//app.Use(async (context, next) =>
//{
//    Endpoint enpoint = context.GetEndpoint();
//    Console.WriteLine("2" + enpoint);
//    await next(context);
//});

// created end points
#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints =>{
    // add endpoints

    endpoints.Map("/login",async(context) => {
        await context.Response.WriteAsync("login");
    } );

    endpoints.MapGet("/about", async (context) =>{
        await context.Response.WriteAsync("about"); 
    });

    endpoints.MapGet("/profile/{username = gaurav}", async (context) => // default value
    {
        string? username = context.Request.RouteValues["username"].ToString();
        await context.Response.WriteAsync($"Profile : {username}");
    });
    
    endpoints.MapGet("/files/{filename}.{extension}", async (context) =>
    {
        await context.Response.WriteAsync($"Files/{context.Request.RouteValues["filename"]}.{context.Request.RouteValues["extension"]}");
    });

    endpoints.MapGet("products/details/{id?}", async (context) => // make a optional parameter
    {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            await context.Response.WriteAsync($"Product Id : {context.Request.RouteValues["id"]}");
        }
        else
        {
            await context.Response.WriteAsync("Product Id not provided");
        }
    });

    endpoints.MapGet("employees/details/{id:int?}", async (context)=>
    {
        await context.Response.WriteAsync($"Employee id : {context.Request.RouteValues["id"]}");
    });
    
    endpoints.MapGet("employees/name/{name:minlength(3)}", async (context)=>
    {
        await context.Response.WriteAsync($"Employee Name : {context.Request.RouteValues["name"]}");
    });

    endpoints.MapGet("employees/department/{name:length(3 , 8 ):alpha}", async (context) => // min and max both are in same / alpha only accept a alphanumeric value
    {
        await context.Response.WriteAsync($"Employee Department : {context.Request.RouteValues["name"]}");
    });

    endpoints.MapGet("employees/mobileNo/{name:length(10):int}", async (context) => // exact 10 match
    {
        await context.Response.WriteAsync($"Employee Mobile No : {context.Request.RouteValues["name"]}");
    });

    endpoints.MapGet("employees/age/{age:range(18 , 35):int}", async (context) => // accept between range
    {
        await context.Response.WriteAsync($"Employee Age : {context.Request.RouteValues["age"]}");
    });


    endpoints.MapGet("daily-digest-report/{reportdate:datetime?}", async (context)=>
    {
        if (context.Request.RouteValues.ContainsKey("reportdate"))
        {
            DateTime reoprtDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
            await context.Response.WriteAsync($"Daily Digest Report for {reoprtDate}");
        }
        else
        {
            await context.Response.WriteAsync("Showing All Report");
        }
    });

    endpoints.MapGet("cities/{cityId:guid}", async (context) =>
    {
        Guid cityId = Guid.Parse(Convert.ToString(context.Request.RouteValues["cityid"])!);
        await context.Response.WriteAsync($"City Information : {cityId}");
    });

    // sales report URL : sales-report/ 2030/apr
    endpoints.Map("sales-report/{year:int:min(2000)}/{month:regex(^(apr|jul|sep|dec)$)}", async (context) => // its give a error
    {
       // await context.Response.WriteAsync($"City Information : {context.Request.RouteValues["year"]}/{context.Request.RouteValues["month"]}");
       int year = Convert.ToInt32(context.Request.RouteValues["year"]);
       string? month = context.Request.RouteValues["month"].ToString();

        if (month != null && (month == "apr" || month == "jul" || month=="sep" || month=="dec"))
        {
            await context.Response.WriteAsync($"Sales Report for {year} - {month}");
        }
        else
        {
            await context.Response.WriteAsync("No Report Found");
        }  
    });


    // custom Route Constraint
    endpoints.Map("product-report/{year:int:min(2000)}/{month:SalesReportMonth}", async (context) => // its give a error
    {
        // await context.Response.WriteAsync($"City Information : {context.Request.RouteValues["year"]}/{context.Request.RouteValues["month"]}");
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = context.Request.RouteValues["month"].ToString();
        await context.Response.WriteAsync($"Product Report for {year} - {month}");

    });

    // URL selection order
    endpoints.Map("sales-report/2024/apr", async context => // higher order than "sales-report/{year:int:min(2000)}/{month:regex(^(apr|jul|sep|dec)$)}"
    {
        await context.Response.WriteAsync("Sales Report for 2024 April");
    });


});
#pragma warning restore ASP0014 // Suggest using top level route registrations


app.Run(async (context) =>
{
    await context.Response.WriteAsync($"Request Recived at {context.Request.Path}");
});

app.Run();
