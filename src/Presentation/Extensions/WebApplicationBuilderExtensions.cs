namespace UrlShortener.Presentation.Extensions;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using Serilog;

[ExcludeFromCodeCoverage]
public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureApplicationBuilder(
        this WebApplicationBuilder builder
    )
    {
        #region Logging

        _ = builder.Host.UseSerilog(
            (hostContext, loggerConfiguration) =>
            {
                var assembly = Assembly.GetEntryAssembly();

                _ = loggerConfiguration
                    .ReadFrom.Configuration(hostContext.Configuration)
                    .Enrich.WithProperty(
                        "Assembly Version",
                        assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version
                    )
                    .Enrich.WithProperty(
                        "Assembly Informational Version",
                        assembly
                            ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                            ?.InformationalVersion
                    );
            }
        );

        #endregion Logging

        #region Serialisation

        _ = builder.Services.Configure<JsonOptions>(opt =>
        {
            opt.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            opt.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            opt.SerializerOptions.PropertyNameCaseInsensitive = true;
            opt.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            opt.SerializerOptions.Converters.Add(
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            );
        });

        #endregion Serialisation

        #region Swagger

        var ti = CultureInfo.CurrentCulture.TextInfo;

        _ = builder.Services.AddEndpointsApiExplorer();

        _ = builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            options.AddDocumentTransformer(
                (document, _, _) =>
                {
                    document.Info = new OpenApiInfo
                    {
                        Version = "v1",
                        Title =
                            $"UrlShortener API - {ti.ToTitleCase(builder.Environment.EnvironmentName)}",
                        Description =
                            "An example to share an implementation of Minimal API in .NET 10.",
                        Contact = new OpenApiContact
                        {
                            Name = "UrlShortener API",
                            Url = new Uri("https://github.com/stphnwlsh/cleanminimalapi"),
                        },
                        License = new OpenApiLicense
                        {
                            Name = "UrlShortener API - License - MIT",
                            Url = new Uri("https://opensource.org/licenses/MIT"),
                        },
                    };
                    return Task.CompletedTask;
                }
            );
        });

        #endregion Swagger

        #region Validation

        _ = builder.Services.AddValidatorsFromAssembly(
            Assembly.GetExecutingAssembly(),
            ServiceLifetime.Singleton
        );

        #endregion Validation

        #region Project Dependencies

        _ = builder.Services.AddInfrastructure(builder.Configuration);
        _ = builder.Services.AddApplication();

        #endregion Project Dependencies

        return builder;
    }
}

internal sealed class BearerSecuritySchemeTransformer(
    IAuthenticationSchemeProvider authenticationSchemeProvider
) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken
    )
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
        if (
            authenticationSchemes.Any(authScheme =>
                authScheme.Name == IdentityConstants.BearerScheme
            )
        )
        {
            var securitySchemes = new Dictionary<string, IOpenApiSecurityScheme>
            {
                [IdentityConstants.BearerScheme] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                },
            };
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = securitySchemes;

            foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations!))
            {
                operation.Value.Security ??= [];
                operation.Value.Security.Add(
                    new OpenApiSecurityRequirement
                    {
                        [
                            new OpenApiSecuritySchemeReference(
                                IdentityConstants.BearerScheme,
                                document
                            )
                        ] = [],
                    }
                );
            }
        }
    }
}
