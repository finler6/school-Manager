using SchoolManage.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace SchoolManage.DAL.Seeds;

public static class ActivitySeeds
{
    public static readonly ActivityEntity one = new()
    {
        Id = Guid.NewGuid(),
        Start = DateTime.Now,
        End = DateTime.Now.AddHours(1),
        Room = Common.Enums.Room.Auditorium,
        Type = Common.Enums.ActivityType.Exam,
        Description = "First activity",
        Subjects = SubjectSeeds.math,
        SubjectsId = SubjectSeeds.math.Id
    };

    public static readonly ActivityEntity two = new()
    {
        Id = Guid.NewGuid(),
        Start = DateTime.Now,
        End = DateTime.Now.AddHours(1),
        Room = Common.Enums.Room.Classroom,
        Type = Common.Enums.ActivityType.Homework,
        Description = "Second activity",
        Subjects = SubjectSeeds.english,
        SubjectsId = SubjectSeeds.english.Id
    };

    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityEntity>().HasData(
            one with { Subjects = null! },
            two with { Subjects = null! }
        );
    }

}