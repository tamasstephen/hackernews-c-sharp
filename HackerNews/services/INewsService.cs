namespace HackerNews.Services;

interface INewsService
{
    Task<List<GetStoriesResponse>> GetStories(int? count);
}

