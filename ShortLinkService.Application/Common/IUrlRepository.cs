using ShortLinkService.Core.Entities;

namespace ShortLinkService.Application.Common;

public interface IUrlRepository
{
    Task<Url?> GetByCodeAsync(string code, CancellationToken ct);
    
    Task<IEnumerable<Url>> GetAllAsync(CancellationToken ct);
    
    Task AddAsync(Url mapping, CancellationToken ct);

    Task UpdateAsync(long id, string newLongUrl, CancellationToken ct);
    
    Task DeleteAsync(long id, CancellationToken ct);
    
    Task IncrementClickCountAsync(string code, CancellationToken ct); 

    Task<Url?> GetByLongUrlAsync(string longUrl, CancellationToken ct);
}