var builder = WebApplication.CreateBuilder();
builder.Services.AddTransient<IHelloService, UaHelloService>();
builder.Services.AddTransient<IHelloService, EnHelloService>();

var app = builder.Build();

app.UseMiddleware<HelloMiddleware>();

app.Run();


interface IHelloService
{
    string Message { get; }
}

class UaHelloService : IHelloService
{
    public string Message => "Çהמנמג METANIT.COM";
}
class EnHelloService : IHelloService
{
    public string Message => "Hello METANIT.COM";
}

class HelloMiddleware
{
    readonly IEnumerable<IHelloService> helloServices;

    public HelloMiddleware(RequestDelegate _, IEnumerable<IHelloService> helloServices)
    {
        this.helloServices = helloServices;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.ContentType = "text/html; charset=utf-8";
        string responseText = "";
        foreach (var service in helloServices)
        {
            responseText += $"<h3>{service.Message}</h3>";
        }
        await context.Response.WriteAsync(responseText);
    }
}