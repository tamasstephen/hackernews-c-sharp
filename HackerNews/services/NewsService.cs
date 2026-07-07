using System.Text.Json;

namespace HackerNews.Services;
    
class NewsService:INewsService
{

    private static readonly int DefaultCount = 10;
    private List<int> _storyIds = [];
    private List<Stories> _stories = [];
    private readonly IHttpClientFactory _clientFactory;

    public NewsService(IHttpClientFactory clientFactory)
    {
       _clientFactory = clientFactory;
    }

    public async Task<StoriesResponse> GetStories(int? count)
    {
        if(_storyIds.Count == 0)
        {
            await GetStoryIdsAsync();
        }

        if(_stories.Count == 0 || count > _stories.Count)
        {
            await GetStoriesAsync(count);
        }

        return new StoriesResponse(_stories, _storyIds.Count, _stories.Count);
    }

    private async Task GetStoryIdsAsync()
    {
        var httpRequestMessage = "https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty";
        var client = _clientFactory.CreateClient();
        var response = await client.GetFromJsonAsync<List<int>>(httpRequestMessage);

        _storyIds = response ?? [];
   }

   private async Task GetStoriesAsync(int? count)
   {

    var tasks = new List<Task<Stories?>>();
    var client = _clientFactory.CreateClient();
    var currentCount = count ?? DefaultCount;
    var start = currentCount == DefaultCount ? 0 : _stories.Count;

    for (int i = start; i < start + currentCount; i++)
    {
        var taskId = _storyIds[i];

        {
            count --;
            var url = $"https://hacker-news.firebaseio.com/v0/item/{taskId}.json?print=pretty";
            tasks.Add(client.GetFromJsonAsync<Stories>(url));    
            if (count == 0)
            {
                break;
            }
        }
    }

    _stories = (await Task.WhenAll(tasks)).Where(story => story is not null).Cast<Stories>().ToList();
   }
}
