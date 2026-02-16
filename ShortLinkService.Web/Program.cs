using Microsoft.EntityFrameworkCore;
using ShortLinkService.Application.Commands.CreateShortUrl;
using ShortLinkService.Application.Common;
using ShortLinkService.Application.Common.Mappings;
using ShortLinkService.Infrastructure.Data;
using ShortLinkService.Infrastructure.Repositories;
using ShortLinkService.Infrastructure.Services;
using ShortLinkService.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IUrlRepository, UrlRepository>();
builder.Services.AddSingleton<IShortenerService, ShortenerService>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateShortUrlCommandHandler).Assembly));

builder.Services.AddAutoMapper(cfg => 
{
    cfg.AddMaps(typeof(MappingProfile).Assembly);
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// 2. Авто-миграция
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    db.Database.Migrate(); 
}

app.UseDefaultFiles(); 
app.UseStaticFiles(); 

app.UseRouting();

app.MapControllers(); 

app.MapFallbackToFile("index.html"); 

app.Run();