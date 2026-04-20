namespace UrlShortener.Infrastructure.Tests.Integration.Databases.UrlShortener.Extensions;

using System;
using Infrastructure.Databases.UrlShortener;
using Infrastructure.Databases.UrlShortener.Models;

internal static class MovieReviewsDbContextExtensions
{
    public static ApplicationDbContext AddTestData(this ApplicationDbContext context)
    {
        List<Author> authors =
        [
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "One",
                LastName = "One",
            },
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "Two",
                LastName = "Two",
            },
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "Three",
                LastName = "Three",
            },
        ];

        context.Authors.AddRange(authors);

        List<Movie> movies =
        [
            new() { Id = Guid.NewGuid(), Title = "One" },
            new() { Id = Guid.NewGuid(), Title = "Two" },
            new() { Id = Guid.NewGuid(), Title = "Three" },
        ];

        context.Movies.AddRange(movies);

        List<Review> reviews =
        [
            new()
            {
                Id = Guid.NewGuid(),
                ReviewAuthorId = authors[0].Id,
                ReviewedMovieId = movies[0].Id,
                Stars = 1,
            },
            new()
            {
                Id = Guid.NewGuid(),
                ReviewAuthorId = authors[0].Id,
                ReviewedMovieId = movies[1].Id,
                Stars = 2,
            },
            new()
            {
                Id = Guid.NewGuid(),
                ReviewAuthorId = authors[0].Id,
                ReviewedMovieId = movies[2].Id,
                Stars = 3,
            },
            new()
            {
                Id = Guid.NewGuid(),
                ReviewAuthorId = authors[1].Id,
                ReviewedMovieId = movies[0].Id,
                Stars = 4,
            },
            new()
            {
                Id = Guid.NewGuid(),
                ReviewAuthorId = authors[1].Id,
                ReviewedMovieId = movies[1].Id,
                Stars = 5,
            },
            new()
            {
                Id = Guid.NewGuid(),
                ReviewAuthorId = authors[1].Id,
                ReviewedMovieId = movies[2].Id,
                Stars = 4,
            },
            new()
            {
                Id = Guid.NewGuid(),
                ReviewAuthorId = authors[2].Id,
                ReviewedMovieId = movies[0].Id,
                Stars = 3,
            },
            new()
            {
                Id = Guid.NewGuid(),
                ReviewAuthorId = authors[2].Id,
                ReviewedMovieId = movies[1].Id,
                Stars = 2,
            },
            new()
            {
                Id = Guid.NewGuid(),
                ReviewAuthorId = authors[2].Id,
                ReviewedMovieId = movies[2].Id,
                Stars = 1,
            },
        ];

        context.Reviews.AddRange(reviews);

        _ = context.SaveChanges();

        return context;
    }
}
