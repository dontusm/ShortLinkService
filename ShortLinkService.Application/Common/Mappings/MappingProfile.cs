using AutoMapper;
using ShortLinkService.Application.Dtos;
using ShortLinkService.Core.Entities;

namespace ShortLinkService.Application.Common.Mappings;

public class MappingProfile : Profile 
{
    public MappingProfile()
    {
        CreateMap<Url, UrlDto>();
    }
}