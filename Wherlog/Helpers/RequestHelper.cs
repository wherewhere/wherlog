using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Wherlog.Models;
using Wherlog.Models.Cate;
using Wherlog.Models.Page;
using Wherlog.Models.Post;

namespace Wherlog.Helpers
{
    public sealed class RequestHelper(string baseUrl)
    {
        public const string DefaultBaseUrl = "https://wherewhere.github.io";

        public static RequestHelper Default { get; set; }

        public HttpClient HttpClient { get; } = new HttpClient { BaseAddress = new Uri(baseUrl) };

        public Task<Entry<T>> GetAsync<T>(string url, CancellationToken cancellationToken = default) =>
            HttpClient.GetFromJsonAsync<Entry<T>>(url, cancellationToken);

        public Task<Entry<T>> GetAsync<T>(string url, JsonTypeInfo<Entry<T>> jsonTypeInfo, CancellationToken cancellationToken = default) =>
            HttpClient.GetFromJsonAsync(url, jsonTypeInfo, cancellationToken);

        public Task<Entry<SiteModel>> GetSiteAsync(CancellationToken cancellationToken = default) =>
            GetAsync("api/site.json", SourceGenerationContext.Default.EntrySiteModel, cancellationToken);

        public Task<Entry<CatesModel>> GetCatesAsync(CateType type, CancellationToken cancellationToken = default) =>
            GetAsync($"api/{type switch { CateType.Category => "categories", CateType.Tag => "tags", _ => throw new NotImplementedException() }}.json", SourceGenerationContext.Default.EntryCateIndexModelArray, cancellationToken);

        public Task<Entry<CateDetailModel>> GetCateAsync(CateType type, string slug, CancellationToken cancellationToken = default) =>
            GetAsync($"api/{type switch { CateType.Category => "categories", CateType.Tag => "tags", _ => throw new NotImplementedException() }}/{slug}.json", SourceGenerationContext.Default.EntryCateDetailModel, cancellationToken);

        public Task<Entry<PostsModel>> GetPostsAsync(CancellationToken cancellationToken = default) =>
            GetAsync("api/posts.json", SourceGenerationContext.Default.EntryPostsModel, cancellationToken);

        public Task<Entry<PostsIndexModel>> GetPostsAsync(int index, CancellationToken cancellationToken = default) =>
            GetAsync($"api/posts/page.{index}.json", SourceGenerationContext.Default.EntryPostsIndexModel, cancellationToken);

        public Task<Entry<PostDetailModel>> GetPostAsync(string path, CancellationToken cancellationToken = default) =>
            GetAsync($"api/posts/{path}.json", SourceGenerationContext.Default.EntryPostDetailModel, cancellationToken);

        public Task<Entry<ArchivesModel>> GetArchivesAsync(CancellationToken cancellationToken = default) =>
            GetAsync("api/archives.json", SourceGenerationContext.Default.EntryYearModelArray, cancellationToken);

        public Task<Entry<YearDetailModel>> GetArchivesAsync(int year, CancellationToken cancellationToken = default) =>
            GetAsync($"api/archives/{year}.json", SourceGenerationContext.Default.EntryArchiveDetailModelYearInfo, cancellationToken);

        public Task<Entry<MonthDetailModel>> GetArchivesAsync(int year, int month, CancellationToken cancellationToken = default) =>
            GetAsync($"api/archives/{year}/{month:D2}.json", SourceGenerationContext.Default.EntryArchiveDetailModelMonthInfo, cancellationToken);

        public Task<Entry<PagesModel>> GetPagesAsync(CancellationToken cancellationToken = default) =>
            GetAsync("api/pages.json", SourceGenerationContext.Default.EntryPageModelArray, cancellationToken);

        public Task<Entry<PageDetailModel>> GetPageAsync(string path, CancellationToken cancellationToken = default) =>
            GetAsync($"api/pages/{path}.json", SourceGenerationContext.Default.EntryPageDetailModel, cancellationToken);
    }

    public static class RequestExtensions
    {
        public static Task<T> GetResults<T>(this Task<Entry<T>> task) where T : class => task.ContinueWith(x => x.Result?.Data);
    }

    [JsonSerializable(typeof(Entry<SiteModel>))]
    [JsonSerializable(typeof(Entry<CatesModel>))]
    [JsonSerializable(typeof(Entry<CateDetailModel>))]
    [JsonSerializable(typeof(Entry<PostsModel>))]
    [JsonSerializable(typeof(Entry<PostsIndexModel>))]
    [JsonSerializable(typeof(Entry<PostDetailModel>))]
    [JsonSerializable(typeof(Entry<ArchivesModel>))]
    [JsonSerializable(typeof(Entry<YearDetailModel>))]
    [JsonSerializable(typeof(Entry<MonthDetailModel>))]
    [JsonSerializable(typeof(Entry<PagesModel>))]
    [JsonSerializable(typeof(Entry<PageDetailModel>))]
    public partial class SourceGenerationContext : JsonSerializerContext;
}
