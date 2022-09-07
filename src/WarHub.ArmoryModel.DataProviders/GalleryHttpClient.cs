
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace WarHub.ArmoryModel.DataProviders;

public sealed class GalleryCache
{
    private Dictionary<string, CatpkgGalleryCache> GalleryInfoCache { get; } = new();

    public IEnumerable<GalleryReference> GalleryReferences => GalleryInfoCache.Values.Select(x => x.Reference);

    public CatpkgGalleryCache? this[string name] => GalleryInfoCache.TryGetValue(name, out var val) ? val : null;

    public CatpkgGalleryCache? this[GalleryReference reference] => this[reference.Name];

    public void Upsert(CatpkgGalleryCache cache)
    {
        GalleryInfoCache[cache.Reference.Name] = cache;
    }
}

public sealed class GalleryBrowserOptions
{
    public List<GalleryReferenceDto> DefaultGalleries { get; } = new();

    public class GalleryReferenceDto
    {
        public string? Name { get; set; }
        public Uri? CatpkgUrl { get; set; }

        public GalleryReference ToRecord() => new(Name!, CatpkgUrl!);
    }
}

public sealed class GalleryBrowserState
{
    public GalleryCache Cache { get; set; } = new();
}

public static class GalleryCacheExtensions
{
    public static async Task<CatpkgGalleryCache> GetHydratedCatpkgGalleryCacheAsync(this GalleryCache cache, GalleryHttpClient client, GalleryReference gallery)
    {
        await cache.GetCatpkgGalleryInfoAsync(client, gallery);
        return cache[gallery] ?? throw new InvalidOperationException("GetCatpkgGalleryInfoAsync failed to update cache.");
    }

    public static async Task<CatpkgGalleryInfo> GetCatpkgGalleryInfoAsync(this GalleryCache cache, GalleryHttpClient client, GalleryReference gallery)
    {
        if (cache[gallery] is { InfoCache: { } info })
        {
            return info;
        }
        var result = await client.GetGalleryFromUrl(gallery.CatpkgUrl);
        cache.Upsert(new CatpkgGalleryCache(gallery) { InfoCache = result ?? throw new InvalidOperationException("Failed to retrieve gallery catpkg.") });
        return result;
    }

    public static async Task<CatpkgRepositoryInfo> GetHydratedCatpkgAsync(this GalleryCache cache, GalleryHttpClient client, RepositoryReference repositoryRef)
    {
        var gallery = await cache.GetHydratedCatpkgGalleryCacheAsync(client, repositoryRef.Gallery);
        if (gallery.RepositoryInfoCache.TryGetValue(repositoryRef, out var repoInfo))
        {
            return repoInfo;
        }
        var repoItem = gallery.InfoCache?.Repositories?.First(x => x.Name == repositoryRef.Name)
            ?? throw new InvalidOperationException("Failed to find referenced repository item.");
        var result = await client.GetCatpkgFromUrl(repoItem.RepositoryUrl ?? throw new InvalidOperationException("Repository has no repositoryUrl."));
        cache.Upsert(gallery with
        {
            RepositoryInfoCache = gallery.RepositoryInfoCache.SetItem(repositoryRef, result)
        });
        return result;
    }
}

public record CatpkgGalleryCache(GalleryReference Reference)
{
    public CatpkgGalleryInfo? InfoCache { get; init; }

    public ImmutableDictionary<RepositoryReference, CatpkgRepositoryInfo> RepositoryInfoCache { get; init; }
        = ImmutableDictionary<RepositoryReference, CatpkgRepositoryInfo>.Empty;
}

public record GalleryReference(string Name, Uri CatpkgUrl);

public record RepositoryReference(string Name, GalleryReference Gallery);

public sealed class GalleryHttpClient : IDisposable
{
    private readonly HttpClient client = new();

    public GalleryHttpClient(IOptions<Options> options)
    {
        client.BaseAddress = new Uri(options.Value.CorsProxyUrl!);
    }

    public void Dispose()
    {
        ((IDisposable)client).Dispose();
    }

    public async Task<CatpkgGalleryInfo> GetGalleryFromUrl(Uri url) => await GetGalleryFromUrl(url.ToString());

    public async Task<CatpkgGalleryInfo> GetGalleryFromUrl(string url)
    {
        var result = await client.GetFromJsonAsync<CatpkgGalleryInfo>("?" + url);
        return result!;
    }

    public async Task<CatpkgRepositoryInfo> GetCatpkgFromUrl(string url)
    {
        var result = await client.GetFromJsonAsync<CatpkgRepositoryInfo>("?" + url);
        return result!;
    }

    public class Options
    {
        public string? CorsProxyUrl { get; set; } = "https://phalanx-cors.bsdata.workers.dev";
    }
}

public record CatpkgGalleryInfo
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? BattleScribeVersion { get; init; }
    public string? DiscordUrl { get; init; }
    public string? TwitterUrl { get; init; }
    public string? FacebookUrl { get; init; }
    public string? RepositorySourceUrl { get; init; }
    public string? FeedUrl { get; init; }
    public string? GithubUrl { get; init; }
    public string? WebsiteUrl { get; init; }
    public ImmutableList<CatpkgRepositoryInfo> Repositories { get; init; } =
        ImmutableList<CatpkgRepositoryInfo>.Empty;
}

public record CatpkgRepositoryInfo
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? BattleScribeVersion { get; init; }
    public string? Version { get; init; }
    public string? LastUpdated { get; init; }
    public string? LastUpdateDescription { get; init; }
    public string? IndexUrl { get; init; }
    public string? RepositoryUrl { get; init; }
    public string? RepositoryGzipUrl { get; init; }
    public string? RepositoryBsrUrl { get; init; }
    public string? GithubUrl { get; init; }
    public string? FeedUrl { get; init; }
    public string? BugTrackerUrl { get; init; }
    public string? ReportBugUrl { get; init; }
    public bool? Archived { get; init; }
    public ImmutableList<CatpkgRepositoryInfo> RepositoryFiles { get; init; } =
        ImmutableList<CatpkgRepositoryInfo>.Empty;
}

[JsonSerializable(typeof(ImmutableList<string>))]
public partial class CatpkgSerializerContext : JsonSerializerContext
{
}

public record CatpkgFileInfo
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Type { get; init; }
    public int? Revision { get; init; }
    public string? BattleScribeVersion { get; init; }
    public string? FileUrl { get; init; }
    public string? GithubUrl { get; init; }
    public string? BugTrackerUrl { get; init; }
    public string? ReportBugUrl { get; init; }
    public string? AuthorName { get; init; }
    public string? AuthorContact { get; init; }
    public string? AuthorUrl { get; init; }
    public string? SourceSha256 { get; init; }
}
