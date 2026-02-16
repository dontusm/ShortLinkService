using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShortLinkService.Core.Entities;

namespace ShortLinkService.Infrastructure.Data.Configurations;

public class UrlMappingConfiguration : IEntityTypeConfiguration<Url>
{
    public void Configure(EntityTypeBuilder<Url> builder)
    {
        builder.HasIndex(u => u.ShortCode)
            .IsUnique();
        
        builder.HasIndex(u => u.LongUrl)
            .IsUnique();
        
        builder.Property(u => u.ShortCode)
            .HasMaxLength(10)
            .IsRequired();
        
        builder.Property(u => u.LongUrl)
            .IsRequired();
    }
}