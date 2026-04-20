namespace UrlShortener.Infrastructure.Tests.Integration.Databases.UrlShortener;

using AutoMapper;
using Xunit;

[Collection("MovieReviews")]
public class MappingConfigurationTests(MovieReviewsDataFixture fixture)
{
    private readonly IMapper mapper = fixture.Mapper;

    [Fact]
    public void ShouldHaveValidMappingConfiguration()
    {
        this.mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}
