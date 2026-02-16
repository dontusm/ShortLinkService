using MediatR;
using ShortLinkService.Application.Dtos;

namespace ShortLinkService.Application.Queries.GetUrlByCode;

public record GetUrlByCodeQuery(string Code) : IRequest<UrlDto>;