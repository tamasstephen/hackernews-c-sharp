using HackerNews.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddSingleton<INewsService, NewsService>();

var app = builder.Build();


app.MapGet("/", async (INewsService newsService) =>
{
   var ids = await newsService.GetStories(10);
   var storyTitles = string.Join(',', ids.Select(id => id.Title).ToList());
   return storyTitles;
});

app.Run();
