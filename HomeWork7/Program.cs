using HomeWork7;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(x =>
{
    x.Filters.Add(typeof(PirateFilter));
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddSingleton<ICrew, Crew>();
builder.Services.AddScoped<IPiratesRepository, PiratesRepository>();

builder.Services.AddDbContext<ApplicationContext>(
        options => options.UseSqlite("name=ConnectionStrings:DefaultConnection"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//app.Use(async (context, next) =>
//{
//    try
//    {
//        Console.WriteLine($"Before: path is {context.Request.Path} and method is {context.Request.Method}");
//        await next();
//        Console.WriteLine($"After: path is {context.Request.Path} and method is {context.Request.Method}");
//    }
//    catch (Exception e)
//    {
//        Console.WriteLine(e.Message);
//    }
//});

app.MapControllers();

//app.MapGet("v2/byName/{name}",
//    (HttpContext requestDelegate) =>
//    {
//        var name = requestDelegate.GetRouteValue("name").ToString();
//        var service = requestDelegate.RequestServices.GetService<ICrew>();
//        var pirate = service.GetByName(name);
//        if (pirate == null) return Results.NoContent();
//        return Results.Ok(pirate);
//    }
//    ).WithName("minAPI").WithOpenApi();

app.Run();
