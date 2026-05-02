namespace UrlShortener.Infrastructure.Databases.UrlShortener.Models;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

public sealed class PaginatedList<T>
{
    public IReadOnlyCollection<T> Items { get; }

    public int PageNumber { get; }

    public int TotalPages { get; }

    public int TotalCount { get; }

    private int PageSize { get; }

    public PaginatedList(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
    {
        this.PageSize = pageSize;
        this.PageNumber = pageNumber;
        this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        this.TotalCount = count;
        this.Items = items;
    }

    public bool HasPreviousPage => this.PageNumber > 1;

    public bool HasNextPage => this.PageNumber < this.TotalPages;

    [SuppressMessage("Design", "CA1000:Do not declare static members on generic types")]
    public static async Task<PaginatedList<T>> CreateAsync(
        IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken: cancellationToken);

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }

    public PaginatedList<TResult> Map<TResult>(Func<T, TResult> mapper)
    {
        return new PaginatedList<TResult>(
            this.Items.Select(mapper).ToList(),
            this.TotalCount,
            this.PageNumber,
            this.PageSize
        );
    }
}

public static class Extensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken
    )
        where TDestination : class
    {
        return PaginatedList<TDestination>.CreateAsync(
            queryable.AsNoTracking(),
            pageNumber,
            pageSize,
            cancellationToken
        );
    }
}
