namespace UrlShortener.Infrastructure.Databases.UrlShortener.Mapping;

using AutoMapper;
using Application = Application.Urls.Entities;
using Infrastructure = Models;

internal class UrlMappingProfile : Profile
{
    public UrlMappingProfile()
    {
        _ = this.CreateMap<Application.Url,Infrastructure.Url>()
            .ForMember(d => d.DateCreated, o => o.Ignore())
            .ForMember(d => d.DateModified, o => o.Ignore())
            .ReverseMap();
    }
}
