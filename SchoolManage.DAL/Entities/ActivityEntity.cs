using SchoolManage.Common.Enums;

namespace SchoolManage.DAL.Entities;

public record ActivityEntity : IEntity
{
    public required DateTime Start { get; set; }
    public required DateTime End { get; set; }
    public required Room Room { get; set; }
    public required ActivityType Type { get; set; }

    public string? Description { get; set; }

    public required SubjectEntity Subjects { get; init; }
    public ICollection<EvaluationEntity> Evaluations { get; set; } = new List<EvaluationEntity>();


    public required Guid SubjectsId { get; set; }
    public required Guid Id { get; set; }
}