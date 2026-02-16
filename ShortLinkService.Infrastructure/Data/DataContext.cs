using Microsoft.EntityFrameworkCore;
using ShortLinkService.Core.Entities;
using ShortLinkService.Infrastructure.Data.Configurations;

namespace ShortLinkService.Infrastructure.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    
    public DbSet<Url> Urls{ get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UrlMappingConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}