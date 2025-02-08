using System;
using SchoolManage.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace SchoolManage.Common.Tests.Seeds;

public static class StudentSeeds
{
    public static readonly StudentEntity EmptyStudentSeed = new()
    {
        Id = default,
        Name = default!,
        Surname = default!,
        ImageUrl = default!,
    };

    public static readonly StudentEntity student1 = new()
    {
        Id = Guid.Parse("87833e66-05ba-4d6b-900b-fe5ace88dbd8"),
        Name = "John",
        Surname = "Doe",
        ImageUrl = @"https://www.pngitem.com/pimgs/m/40-406527_cartoon-glass-of-water-png-glass-of-water.png",
    };

    public static readonly StudentEntity student2 = new()
    {
        Id = Guid.Parse("1187033e-05ba-4d6b-900b-fe5ace88abd8"),
        Name = "Jane",
        Surname = "Doe",
        ImageUrl = @"https://www.eatthis.com/wp-content/uploads/sites/4/2020/02/lemons.jpg?quality=82&strip=1&resize=640%2C360"
    };

    //To ensure that no tests reuse these clones for non-idempotent operations
    public static readonly StudentEntity StudentEntityUpdate = student1 with { Id = Guid.Parse("0953F3CE-7B1A-48C1-9796-D2BAC7F67868"), Subjects = Array.Empty<SubjectEntity>(), Evaluations = Array.Empty<EvaluationEntity>() };
    public static readonly StudentEntity StudentEntityDelete = student2 with { Id = Guid.Parse("5DCA4CEA-B8A8-4C86-A0B3-FFB78FBA1A09"), Subjects = Array.Empty<SubjectEntity>(), Evaluations = Array.Empty<EvaluationEntity>() };

    public static void Seed(ModelBuilder modelBuilder) => modelBuilder.Entity<StudentEntity>().HasData(
        student1,
        student2 with { Subjects = Array.Empty<SubjectEntity>(), Evaluations = Array.Empty<EvaluationEntity>() },
        StudentEntityUpdate,
        StudentEntityDelete
    );
}