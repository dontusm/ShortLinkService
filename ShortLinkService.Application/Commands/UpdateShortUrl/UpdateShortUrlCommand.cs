using MediatR;

namespace ShortLinkService.Application.Commands.UpdateShortUrl;

public record UpdateShortUrlCommand(long Id, string NewLongUrl) : IRequest; 