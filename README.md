# HackerNews Simple Server

A minimal ASP.NET Core Web API built as a learning project for getting familiar with C#. It exposes a single endpoint that fetches top stories from the [Hacker News API](https://github.com/HackerNews/API).

## What it does

- Calls the public Hacker News Firebase API to get the list of top story IDs.
- Fetches the details for a configurable number of stories in parallel.
- Returns them as JSON, along with the total number of available stories and how many were returned.

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

## Running the project

```bash
dotnet run --project HackerNews
```

The server starts at `http://localhost:5000`.

## Usage

```
GET http://localhost:5000/            # returns the default number of stories (10)
GET http://localhost:5000/?count=5    # returns 5 stories
```

Example response:

```json
{
  "data": [
    {
      "id": 12345,
      "title": "Some story title",
      "url": "https://example.com",
      "by": "someuser",
      "score": 100,
      "time": 1700000000,
      "type": "story"
    }
  ],
  "total": 500,
  "count": 5
}
```

You can also try requests using the `HackerNews.http` file with the REST Client extension in VS Code.

## Project structure

```
HackerNews/
├── Program.cs                    # App startup: DI, CORS, route mapping
└── services/
    ├── INewsService.cs           # Service contract
    ├── NewsService.cs            # Fetches story IDs/details from Hacker News
    └── GetStoriesResponse.cs     # Stories / StoriesResponse records
```

## C# / .NET concepts demonstrated

- Minimal APIs (`app.MapGet`)
- Dependency injection (`IHttpClientFactory`, custom services)
- `async`/`await` and parallel tasks (`Task.WhenAll`)
- Records for immutable data models
- Nullable reference types
- CORS configuration
