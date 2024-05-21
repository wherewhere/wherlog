using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Wherlog.Models;
using Wherlog.Models.Page;
using Wherlog.Models.Post;

namespace Wherlog.Helpers
{
    public class RequestHelper(string baseUrl)
    {
        public static RequestHelper Default { get; } = new RequestHelper("https://wherewhere.github.io");

        public HttpClient HttpClient { get; } = new HttpClient { BaseAddress = new Uri(baseUrl) };

        public Task<Entry<T>> GetAsync<T>(string url, CancellationToken cancellationToken = default) =>
            HttpClient.GetFromJsonAsync<Entry<T>>(url, cancellationToken);

        public Task<Entry<T>> GetAsync<T>(string url, JsonTypeInfo<Entry<T>> jsonTypeInfo, CancellationToken cancellationToken = default) =>
            HttpClient.GetFromJsonAsync(url, jsonTypeInfo, cancellationToken);

        public Task<Entry<SiteModel>> GetSiteAsync(CancellationToken cancellationToken = default) =>
            GetAsync("api/site.json", SourceGenerationContext.Default.EntrySiteModel, cancellationToken);

        public Task<Entry<PostsModel>> GetPostsAsync(CancellationToken cancellationToken = default) =>
            GetAsync("api/posts.json", SourceGenerationContext.Default.EntryPostsModel, cancellationToken);

        public Task<Entry<PostsIndexModel>> GetPostsAsync(int index, CancellationToken cancellationToken = default) =>
            GetAsync($"api/posts/page.{index}.json", SourceGenerationContext.Default.EntryPostsIndexModel, cancellationToken);

        public Task<Entry<PagesModel[]>> GetPagesAsync(CancellationToken cancellationToken = default) =>
            GetAsync("api/pages.json", SourceGenerationContext.Default.EntryPagesModelArray, cancellationToken);

        public Task<Entry<PageModel>> GetPageAsync(string path, CancellationToken cancellationToken = default) =>
            GetAsync($"api/pages/{path}.json", SourceGenerationContext.Default.EntryPageModel, cancellationToken);
    }

    [JsonSerializable(typeof(Entry<SiteModel>))]
    [JsonSerializable(typeof(Entry<PostsModel>))]
    [JsonSerializable(typeof(Entry<PostsIndexModel>))]
    [JsonSerializable(typeof(Entry<PagesModel[]>))]
    [JsonSerializable(typeof(Entry<PageModel>))]
    public partial class SourceGenerationContext : JsonSerializerContext;
}
