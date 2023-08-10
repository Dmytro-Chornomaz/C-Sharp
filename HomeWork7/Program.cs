using HomeWork7;
using HomeWork7.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers(x =>
//{
//    x.Filters.Add(typeof(PirateFilter));
//});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      new string[] { }
    }
  });
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var secret = builder.Configuration.GetValue<string>("Auth:Secret");
    var issuer = builder.Configuration.GetValue<string>("Auth:Issuer");
    var audience = builder.Configuration.GetValue<string>("Auth:Audience");
    var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = mySecurityKey
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<ApplicationContext>();

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

app.UseAuthentication();
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
