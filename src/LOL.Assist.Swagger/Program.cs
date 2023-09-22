using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//openApi.json‘⁄œﬂµÿ÷∑ https://www.mingweisamuel.com/lcu-schema/tool/
builder.Services.AddEndpointsApiExplorer();
List<string?> swaggerJsonFiles = Directory.GetFiles($"{Environment.CurrentDirectory}/wwwroot")
    .Select(Path.GetFileName)
    .ToList();
builder.Services.AddSwaggerGen(genOptions =>
{
    foreach (var file in swaggerJsonFiles)
    {
        genOptions.SwaggerDoc(file, new Microsoft.OpenApi.Models.OpenApiInfo()
        {
            Title = file,
            Version = "v"
        });
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(uiOptions =>
{
    foreach (var file in swaggerJsonFiles)
    {
        uiOptions.SwaggerEndpoint($"/{file}", file);
    }

    uiOptions.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
});
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
