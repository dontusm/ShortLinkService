using MediatR;
using ShortLinkService.Application.Common;
using ShortLinkService.Application.Dtos;

namespace ShortLinkService.Application.Queries.GetUrlByCode;

public class GetUrlByCodeQueryHandler(IUrlRepository repository) : IRequestHandler<GetUrlByCodeQuery, UrlDto>
{
    public async Task<UrlDto> Handle(GetUrlByCodeQuery request, CancellationToken ct)
    {
        var mapping = await repository.GetByCodeAsync(request.Code, ct);
        if (mapping == null) return null;
        
        await repository.IncrementClickCountAsync(request.Code, ct);
        
        return new UrlDto
        {
            Id = mapping.Id,
            LongUrl = mapping.LongUrl,
            ShortCode = mapping.ShortCode,
            CreatedAt = mapping.CreatedAt,
            ClickCount =  mapping.ClickCount + 1
        };
    }
}