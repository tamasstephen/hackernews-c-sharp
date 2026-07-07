using HackerNews.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddSingleton<INewsService, NewsService>();
builder.Services.AddCors(options =>
{
   options.AddDefaultPolicy(policy =>
   {
      policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
   });
});

var app = builder.Build();
app.UseCors();

app.MapGet("/", async (int? count, INewsService newsService) =>
{
   var stories = await newsService.GetStories(count);
   return stories;
});

app.Run("http://localhost:5000");
