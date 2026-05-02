namespace UrlShortener.Infrastructure.Databases.UrlShortener;

using Application.Urls.Entities;
using Application.Users.Entities;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

internal sealed partial class DatabaseSeeder(
    UserManager<AppUser> userManager,
    ApplicationDbContext dbContext,
    IHostEnvironment environment,
    ILogger<DatabaseSeeder> logger
)
{
    private const string TestEmail = "test@example.com";
    private const string TestPassword = "abc";
    private const int TestUrlsCount = 1000;

    /// <summary>
    /// Seeds test data. No-ops outside development environment.
    /// </summary>
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (!environment.IsDevelopment())
        {
            return;
        }

        this.LogSeedingDatabase();

        var testUser = await this.SeedUsersAsync();
        await this.SeedUrlsAsync(testUser, cancellationToken);

        this.LogSeedingCompleted();
    }

    private async Task SeedUrlsAsync(AppUser createdBy, CancellationToken cancellationToken)
    {
        if (
            await dbContext.Urls.AnyAsync(it => it.ShortCode.StartsWith("test_"), cancellationToken)
        )
        {
            return;
        }

        var urlFaker = new Faker<Url>()
            .RuleFor(it => it.OriginalUrl, f => f.Internet.Url())
            .RuleFor(it => it.ShortCode, (a) => "test_" + (a.IndexFaker + 1))
            .RuleFor(it => it.CreatedBy, createdBy);

        var urls = urlFaker.Generate(TestUrlsCount);

        dbContext.Urls.AddRange(urls);

        await dbContext.SaveChangesAsync(cancellationToken);

        this.LogUrlsSeeded(TestUrlsCount);
    }

    private async Task<AppUser> SeedUsersAsync()
    {
        var existing = await userManager.FindByEmailAsync(TestEmail);
        if (existing is not null)
        {
            this.LogUserEmailAlreadyExistsSkippingSeed(TestEmail);
            return existing;
        }

        var user = new AppUser { Email = TestEmail, UserName = TestEmail };
        var result = await userManager.CreateAsync(user, TestPassword);

        if (!result.Succeeded)
        {
            throw new SeedUserException(result.Errors);
        }

        this.LogSeededUserEmail(TestEmail);
        return user;
    }

    [LoggerMessage(LogLevel.Information, "User {Email} already exists, skipping seed")]
    partial void LogUserEmailAlreadyExistsSkippingSeed(string email);

    [LoggerMessage(LogLevel.Information, "Seeded user {Email}")]
    partial void LogSeededUserEmail(string email);

    [LoggerMessage(LogLevel.Information, "Seeded {Count} test URLs")]
    partial void LogUrlsSeeded(int count);

    [LoggerMessage(LogLevel.Information, "Seeding database...")]
    partial void LogSeedingDatabase();

    [LoggerMessage(LogLevel.Information, "Seeding completed")]
    partial void LogSeedingCompleted();
}

internal sealed class SeedUserException(IEnumerable<IdentityError> errors)
    : Exception(
        $"Failed to seed test user: {string.Join(", ", errors.Select(e => e.Description))}"
    );
