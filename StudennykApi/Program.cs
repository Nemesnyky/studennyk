using RendleLabs.OpenApi.Web;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseStaticOpenApi(Assembly.GetExecutingAssembly(), "StudennykApi.openapi.yaml",
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
