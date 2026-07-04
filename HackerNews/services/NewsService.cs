using System.Text.Json;

namespace HackerNews.Services;
    
class NewsService:INewsService
{

    private static readonly int DefaultCount = 10;
    private List<int> _storyIds = [];
    private List<GetStoriesResponse> _stories = [];
    private readonly IHttpClientFactory _clientFactory;

    public NewsService(IHttpClientFactory clientFactory)
    {
       _clientFactory = clientFactory;
    }

    public async Task<List<GetStoriesResponse>> GetStories(int? count)
    {
        if(_storyIds.Count == 0)
        {
            await GetStoryIdsAsync();
        }

        if(_stories.Count == 0 || count > _stories.Count)
        {
            await GetStoriesAsync(count);
        }

        return _stories;
    }

    private async Task GetStoryIdsAsync()
    {
        var httpRequestMessage = GetHttpReqestMessage(
            "https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty"
        );
        var client = _clientFactory.CreateClient();
        var httpResponseMessage = await client.SendAsync(httpRequestMessage);
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            throw new HttpRequestException( $"The API returned {(int)httpResponseMessage.StatusCode} ({httpResponseMessage.StatusCode}).",
        null,
        httpResponseMessage.StatusCode);
        }

        var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
        var ids = await JsonSerializer.DeserializeAsync<List<int>>(contentStream);
        _storyIds = ids;
   }

   private async Task GetStoriesAsync(int? count)
   {

    var tasks = new List<Task<GetStoriesResponse?>>();
    var client = _clientFactory.CreateClient();
    var currentCount = count ?? DefaultCount;
    var start = currentCount == DefaultCount ? 0 : _stories.Count;

    for (int i = start; i < start + currentCount; i++)
    {
        var taskId = _storyIds[i];

        {
            count --;
            var url = $"https://hacker-news.firebaseio.com/v0/item/{taskId}.json?print=pretty";
            tasks.Add(client.GetFromJsonAsync<GetStoriesResponse>(url));    
            if (count == 0)
            {
                break;
            }
        }
    }

    _stories = (await Task.WhenAll(tasks)).Where(story => story is not null).Cast<GetStoriesResponse>().ToList();
   }

   private HttpRequestMessage GetHttpReqestMessage(string url)
    {
        return new HttpRequestMessage(
            HttpMethod.Get,
            url
        );
    }

}
