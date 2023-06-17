using System.Net;
using Microsoft.Fast.Components.FluentUI.Infrastructure;

namespace Microsoft.Fast.Components.FluentUI;

public class StaticAssetCache
{
    private readonly Dictionary<string, string> cache = new();

    public string? Get(string key)
    {
        if (cache.TryGetValue(key, out var value))
            return value;
        return null;
    }

    public void Put(string key, string value)
    {
        cache[key] = value;
    }
}

public class FileBasedStaticAssetService : IStaticAssetService
{
    private readonly StaticAssetCache cache;

    public FileBasedStaticAssetService(StaticAssetCache cache)
    {
        this.cache = cache;
    }

    public async Task<string?> GetAsync(string assetUrl, bool useCache = true)
    {
        string? result = null;
        if (useCache)
        {
            // Get the result from the cache
            result = cache.Get(assetUrl);
        }

        if (string.IsNullOrEmpty(result))
        {
            try
            {
                //It not in the cache (or cache not used), download the asset
                result = await ReadData(assetUrl);
                // If successful, store the response in the cache and get the result
                if (useCache)
                    cache.Put(assetUrl, result);
            }
            catch
            {
                result = string.Empty;
            }
        }

        return result;
    }

    private static async Task<string> ReadData(string file)
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync($"wwwroot/{file}");
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}
