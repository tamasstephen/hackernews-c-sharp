using System.Text.Json;

namespace HackerNews.Services;
    
class NewsService:INewsService
{
    private readonly IHttpClientFactory _clientFactory;

    public NewsService(IHttpClientFactory clientFactory)
    {
       _clientFactory = clientFactory;
    }

    public async Task<List<GetStoriesResponse>> GetStories(int count = 10)
    {
        var ids = await GetStoriyIdsAsync();

        return new List<GetStoriesResponse>();
    }

    private async Task<List<int>?> GetStoriyIdsAsync()
    {
        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
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
        List<int> ids = await JsonSerializer.DeserializeAsync<List<int>>(contentStream);
        Console.WriteLine($"The ids are the following:");
        foreach (int id in ids)
        {
            Console.WriteLine(id);
        }
        return ids;
   }

}
