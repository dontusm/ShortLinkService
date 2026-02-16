using MediatR;
using ShortLinkService.Application.Common;

namespace ShortLinkService.Application.Commands.DeleteShortUrl;

public class DeleteShortUrlCommandHandler(IUrlRepository repository) : IRequestHandler<DeleteShortUrlCommand>
{
    public async Task Handle(DeleteShortUrlCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id, cancellationToken);
    }
}