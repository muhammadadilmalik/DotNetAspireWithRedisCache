using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.AddRedisDistributedCache("my-redis-cache");

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/random", async (IDistributedCache cache) =>
{
    const string key = "cached_string";

    var cached = await cache.GetStringAsync(key);
    if (!string.IsNullOrEmpty(cached))
        return Results.Ok(new { value = cached, source = "cache" });

    var random = Guid.NewGuid().ToString();
    await cache.SetStringAsync(key, random, new DistributedCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5)
    });

    return Results.Ok(new { value = random, source = "generated" });
});


app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
