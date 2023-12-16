using EnGram.DB.Entities;

namespace CatalogService.Features.Exercises;

public record ExerciseUpdateViewModel(
    int TopicId,
    ExerciseType Type,
    string Task,
    string Answer);