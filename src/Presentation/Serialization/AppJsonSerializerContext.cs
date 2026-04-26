namespace UrlShortener.Presentation.Serialization;

using System.Text.Json.Serialization;
using Application.Urls.Entities;
using Application.Versions.Entities;
using Infrastructure.Databases.UrlShortener.Models;
using Requests;
using Responses;

/// <summary>
/// JSON serialization context for compile-time source generation.
/// Provides performance benefits and AOT (Ahead-of-Time) compilation support.
/// </summary>
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNameCaseInsensitive = true,
    WriteIndented = false,
    GenerationMode = JsonSourceGenerationMode.Default
)]
[JsonSerializable(typeof(Version))]
[JsonSerializable(typeof(Url))]
[JsonSerializable(typeof(List<Version>))]
[JsonSerializable(typeof(GetUrlsRequest))]
[JsonSerializable(typeof(PaginatedList<UrlApiResponse>))]
[JsonSerializable(typeof(ShortenUrlRequest))]
public partial class AppJsonSerializerContext : JsonSerializerContext;
