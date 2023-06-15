using HomeWork7;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICrew, Crew>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("v2/byName/{name}",
    (HttpContext requestDelegate) =>
    {
        var name = requestDelegate.GetRouteValue("name").ToString();
        var service = requestDelegate.RequestServices.GetService<ICrew>();
        var pirate = service.GetByName(name);
        if (pirate == null) return Results.NoContent();
        return Results.Ok(pirate);
    }
    ).WithName("minAPI").WithOpenApi();

app.Run();
