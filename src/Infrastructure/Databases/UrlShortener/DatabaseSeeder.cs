namespace UrlShortener.Infrastructure.Databases.UrlShortener;

using Application.Users.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

internal sealed partial class DatabaseSeeder(
    UserManager<AppUser> userManager,
    ILogger<DatabaseSeeder> logger
)
{
    private const string TestEmail = "test@example.com";
    private const string TestPassword = "abc";

    public async Task SeedAsync()
    {
        await this.SeedUsersAsync();
    }

    private async Task SeedUsersAsync()
    {
        if (await userManager.FindByEmailAsync(TestEmail) is not null)
        {
            this.LogUserEmailAlreadyExistsSkippingSeed(TestEmail);
            return;
        }

        var user = new AppUser { Email = TestEmail, UserName = TestEmail };
        var result = await userManager.CreateAsync(user, TestPassword);

        if (result.Succeeded)
        {
            this.LogSeededUserEmail(TestEmail);
        }
        else
        {
            this.LogFailedToSeedUserEmailErrors(
                TestEmail,
                string.Join(", ", result.Errors.Select(e => e.Description))
            );
        }
    }

    [LoggerMessage(LogLevel.Information, "User {Email} already exists, skipping seed")]
    partial void LogUserEmailAlreadyExistsSkippingSeed(string email);

    [LoggerMessage(LogLevel.Information, "Seeded user {Email}")]
    partial void LogSeededUserEmail(string email);

    [LoggerMessage(LogLevel.Warning, "Failed to seed user {Email}: {Errors}")]
    partial void LogFailedToSeedUserEmailErrors(string email, string errors);
}
