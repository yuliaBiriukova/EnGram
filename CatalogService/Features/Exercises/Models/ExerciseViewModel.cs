using EnGram.DB.Entities;

namespace CatalogService.Features.Exercises;

public record ExerciseViewModel(
    int Id,
    int TopicId,
    ExerciseType Type,
    string Task,
    string Answer);