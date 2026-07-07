namespace HackerNews.Services;

interface INewsService
{
    Task<StoriesResponse> GetStories(int? count);
}

