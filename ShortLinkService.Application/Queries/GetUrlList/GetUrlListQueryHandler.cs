using MediatR;
using ShortLinkService.Application.Common;
using ShortLinkService.Application.Dtos;

namespace ShortLinkService.Application.Queries.GetUrlList;

public class GetUrlListQueryHandler(IUrlRepository repository) : IRequestHandler<GetUrlListQuery, IEnumerable<UrlDto>>
{
    public async Task<IEnumerable<UrlDto>> Handle(GetUrlListQuery request, CancellationToken cancellationToken)
    {
        var list = await repository.GetAllAsync(cancellationToken);
        return list.Select(x => new UrlDto
        {
            Id = x.Id,
            LongUrl = x.LongUrl,
            ShortCode = x.ShortCode,
            CreatedAt = x.CreatedAt,
            ClickCount = x.ClickCount
        });
    }
}