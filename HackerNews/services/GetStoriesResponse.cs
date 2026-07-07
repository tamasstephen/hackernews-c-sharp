public record Stories(
    int Id,
    string? Title,
    string? Url,
    string? By,
    int Score,
    long Time,
    string? Type
);

public record StoriesResponse (
    List<Stories> Data,
    int Total,
    int Count
);
