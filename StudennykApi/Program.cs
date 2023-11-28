using RendleLabs.OpenApi.Web;
using System.Reflection;
using StudennykApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSingleton<SQLiteRepository>(new SQLiteRepository());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseStaticOpenApi(Assembly.GetExecutingAssembly(), "StudennykAPI.openapi.yaml",
                new StaticOpenApiOptions
                {
                    Version = 1,
                    UiPathPrefix = "swagger",
                    JsonPath = "swagger/v1/openapi.json",
                    YamlPath = "swagger/v1/openapi.yaml",
                    Elements =
                    {
                    Path = "swagger/v1/docs"
                    }
                })
            .AllowAnonymous();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();
