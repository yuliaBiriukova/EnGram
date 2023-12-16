namespace CatalogService.Features.Topics;

public record TopicUpdateModel(
    int LevelId,
    string Name,
    string Content);
