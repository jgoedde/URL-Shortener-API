using Microsoft.EntityFrameworkCore;
using UrlShortener;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContextPool<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "v1"); });
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app
    .MapGet("/{shortCode:required}", async (string shortCode, AppDbContext dbContext) =>
    {
        var url = await dbContext.Urls.FirstAsync(it => it.ShortCode == shortCode); // TODO: Add caching!
        return Results.Redirect(url.LongUrl); // 302
    })
    .WithSummary("Redirects to original URL")
    .WithDescription("Redirects to original URL with HTTP 302 code by short code.");

app.Run();