using API.Errors;
using API.MiddleWare;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddCors();

// Redis configuration
var redisConnString = builder.Configuration.GetConnectionString("Redis") ??
    throw new Exception("Redis connection string not found");
builder.Services.AddSingleton<IConnectionMultiplexer>(config => {
    var configuration = ConfigurationOptions.Parse(redisConnString, true);
    return ConnectionMultiplexer.Connect(configuration);
});
builder.Services.AddSingleton<ICartService, CartService>();

// Add Redis Inspection service
builder.Services.AddSingleton<RedisCartInspection>(_ => new RedisCartInspection(redisConnString));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleWare>();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
    .WithOrigins("https://localhost:4200", "http://localhost:4200"));

app.MapControllers();

// Redis health check endpoint
app.MapGet("/redis-health", (RedisCartInspection redisInspection) =>
{
    var (isHealthy, message) = redisInspection.CheckRedisHealth();
    return Results.Ok(new { IsHealthy = isHealthy, Message = message });
});

// Redis cart inspection endpoint
app.MapGet("/inspect-cart/{cartId}", (string cartId, RedisCartInspection redisInspection) =>
{
    var (success, result) = redisInspection.InspectRedisCart(cartId);
    if (success)
        return Results.Ok(result);
    else
        return Results.BadRequest(result);
});

try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    throw;
}

app.Run();