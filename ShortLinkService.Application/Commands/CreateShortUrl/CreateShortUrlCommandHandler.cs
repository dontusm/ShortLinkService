using AutoMapper;
using MediatR;
using ShortLinkService.Application.Common;
using ShortLinkService.Application.Dtos;
using ShortLinkService.Core.Entities;

namespace ShortLinkService.Application.Commands.CreateShortUrl;

public class CreateShortUrlCommandHandler(
    IUrlRepository repository, 
    IShortenerService shortener, 
    IMapper mapper)
    : IRequestHandler<CreateShortUrlCommand, UrlDto>
{
    public async Task<UrlDto> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
    {
        var existing = await repository.GetByLongUrlAsync(request.LongUrl, cancellationToken);
        if (existing != null) return mapper.Map<UrlDto>(existing);
        
        var mapping = new Url {
            LongUrl = request.LongUrl,
            ShortCode = shortener.GenerateCode(),
            CreatedAt = DateTime.UtcNow
        };

        await repository.AddAsync(mapping, cancellationToken);

        return mapper.Map<UrlDto>(mapping);
    }
}