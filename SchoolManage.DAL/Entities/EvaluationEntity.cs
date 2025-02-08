namespace SchoolManage.DAL.Entities;

public record EvaluationEntity : IEntity
{
    public required string Points { get; set; }
    public string? Description { get; set; }
    public required Guid Id { get; set; }
    public required Guid StudentsId { get; set; }
    public required Guid ActivitiesId { get; set; }
    public required ActivityEntity Activities { get; init; }
    public required StudentEntity Students { get; init; }

}