using MediatR;
using ShortLinkService.Application.Common;

namespace ShortLinkService.Application.Commands.UpdateShortUrl;

public class UpdateUrlHandler(IUrlRepository repository) : IRequestHandler<UpdateShortUrlCommand>
{
    public async Task Handle(UpdateShortUrlCommand request, CancellationToken ct)
    {
        var existing = await repository.GetByLongUrlAsync(request.NewLongUrl, ct);
        
        if (existing != null && existing.Id != request.Id)
        {
            throw new InvalidOperationException("Такая ссылка уже существует в системе.");
        }
        
        await repository.UpdateAsync(request.Id, request.NewLongUrl, ct);
    }
}