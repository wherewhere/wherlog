using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Wherlog.Models.Page;
using Wherlog.Models;
using System.Threading;
using Wherlog.Models.Post;

namespace Wherlog.Helpers
{
    public class RequestHelper(string baseUrl)
    {
        public static RequestHelper Default { get; } = new RequestHelper("https://wherewhere.github.io");

        public HttpClient HttpClient { get; } = new HttpClient { BaseAddress = new Uri(baseUrl) };

        public Task<Entry<T>> GetAsync<T>(string url, CancellationToken cancellationToken = default) =>
            HttpClient.GetFromJsonAsync<Entry<T>>(url, cancellationToken);

        public Task<Entry<PostsModel>> GetPostsAsync(CancellationToken cancellationToken = default) =>
            GetAsync<PostsModel>("api/posts.json", cancellationToken);

        public Task<Entry<PostsIndexModel>> GetPostsAsync(int index, CancellationToken cancellationToken = default) =>
            GetAsync<PostsIndexModel>($"api/posts/page.{index}.json", cancellationToken);

        public Task<Entry<PagesModel[]>> GetPagesAsync(CancellationToken cancellationToken = default) =>
            GetAsync<PagesModel[]>("api/pages.json", cancellationToken);

    }
}
