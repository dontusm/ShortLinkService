using MediatR;

namespace ShortLinkService.Application.Commands.DeleteShortUrl;

public record DeleteShortUrlCommand(long Id) : IRequest;