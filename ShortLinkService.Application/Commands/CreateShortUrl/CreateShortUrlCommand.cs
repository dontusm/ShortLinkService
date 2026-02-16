using MediatR;
using ShortLinkService.Application.Dtos;

namespace ShortLinkService.Application.Commands.CreateShortUrl;

public record CreateShortUrlCommand(string LongUrl) : IRequest<UrlDto>;