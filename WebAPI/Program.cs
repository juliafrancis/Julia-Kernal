using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapGet("/", context =>
    {
        context.Response.Redirect("/scalar/v1", permanent: false);
        return Task.CompletedTask;
    });
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Julia API")
            .WithTheme(ScalarTheme.DeepSpace)
            .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();