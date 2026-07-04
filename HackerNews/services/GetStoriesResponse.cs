public record GetStoriesResponse(
    int Id,
    string? Title,
    string? Url,
    string? By,
    int Score,
    long Time,
    string? Type
);