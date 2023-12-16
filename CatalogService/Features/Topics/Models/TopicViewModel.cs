namespace CatalogService.Features.Topics;

public record TopicViewModel(
    int Id,
    int LevelId,
    string Name,
    string Content);