namespace SchoolManage.DAL.Entities;

public record SubjectEntity : IEntity
{
    public required string Name { get; set; }
    public required string Acronym { get; set; }

    public ICollection<StudentEntity> Students { get; init; } = new List<StudentEntity>();
    public ICollection<ActivityEntity> Activities { get; init; } = new List<ActivityEntity>();

    public required Guid Id { get; set; }
}