using MediatR;
using ShortLinkService.Application.Dtos;

namespace ShortLinkService.Application.Queries.GetUrlList;

public record GetUrlListQuery : IRequest<IEnumerable<UrlDto>>;