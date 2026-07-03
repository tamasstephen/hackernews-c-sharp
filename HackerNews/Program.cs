using HackerNews.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddSingleton<INewsService, NewsService>();

var app = builder.Build();


app.MapGet("/", async (INewsService newsService) =>
{
   var ids = await newsService.GetStories(10);
   Console.WriteLine($"The ids are the following:");
   return $"Hello World!";
});

app.Run();
