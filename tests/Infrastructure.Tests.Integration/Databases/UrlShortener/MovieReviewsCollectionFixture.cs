namespace UrlShortener.Infrastructure.Tests.Integration.Databases.UrlShortener;

using System;
using AutoMapper;
using Extensions;
using Infrastructure.Databases.UrlShortener;
using Infrastructure.Databases.UrlShortener.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;
using Xunit;

[CollectionDefinition("MovieReviews")]
public class MovieReviewsCollectionFixture : ICollectionFixture<MovieReviewsDataFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

public class MovieReviewsDataFixture : IDisposable
{
    internal ApplicationDbContext Context { get; set; }
    internal FakeTimeProvider TimeProvider { get; set; }
    internal IMapper Mapper { get; set; }
    internal EntityFrameworkDefaultRepository Repository { get; set; }

    public MovieReviewsDataFixture()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"TestMovies-{Guid.NewGuid()}")
            .Options;

        this.Context = new ApplicationDbContext(options);

        this.TimeProvider = new FakeTimeProvider();
        this.TimeProvider.SetUtcNow(new DateTime(2009, 12, 31, 23, 51, 01));

        this.Mapper = new MapperConfiguration(cfg =>
            cfg.AddProfiles([
                new AuthorMappingProfile(),
                new MovieMappingProfile(),
                new ReviewMappingProfile(),
            ])
        ).CreateMapper();

        this.Repository = new EntityFrameworkDefaultRepository(
            this.Context,
            this.TimeProvider,
            this.Mapper
        );

        _ = this.Context.Database.EnsureDeleted();
        _ = this.Context.Database.EnsureCreated();
        _ = this.Context.AddTestData();
    }

    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.Context?.Dispose();
            this.Context = null;
        }
    }
}
