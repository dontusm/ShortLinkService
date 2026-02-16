using Microsoft.EntityFrameworkCore;
using ShortLinkService.Application.Common;
using ShortLinkService.Core.Entities;
using ShortLinkService.Infrastructure.Data;

namespace ShortLinkService.Infrastructure.Repositories;

public class UrlRepository(DataContext context) : IUrlRepository
{
    public async Task<Url?> GetByCodeAsync(string code, CancellationToken ct)
    {
        return await context.Urls
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ShortCode == code, ct);
    }

    public async Task<IEnumerable<Url>> GetAllAsync(CancellationToken ct)
    {
        return await context.Urls
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Url mapping, CancellationToken ct)
    {
        context.Urls.Add(mapping);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(long id, string newLongUrl, CancellationToken ct)
    {
         await context.Urls
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(s => s.SetProperty(u => u.LongUrl, newLongUrl), ct);
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        await context.Urls
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(ct);
    }

    public async Task IncrementClickCountAsync(string code, CancellationToken ct)
    {
        await context.Urls
            .Where(x => x.ShortCode == code)
            .ExecuteUpdateAsync(s => s.SetProperty(u => u.ClickCount, u => u.ClickCount + 1), ct);
    }

    public async Task<Url?> GetByLongUrlAsync(string longUrl, CancellationToken ct)
    {
        return await context.Urls
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.LongUrl == longUrl, ct);
    }
}