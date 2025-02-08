namespace SchoolManage.DAL.Entities;

public record StudentEntity : IEntity
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public string? ImageUrl { get; set; }

    public ICollection<SubjectEntity> Subjects { get; set; } = new List<SubjectEntity>();
    public ICollection<EvaluationEntity> Evaluations { get; init; } = new List<EvaluationEntity>();
    public required Guid Id { get; set; }
}