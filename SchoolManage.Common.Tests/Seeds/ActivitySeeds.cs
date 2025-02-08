using System;
using SchoolManage.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using SchoolManage.Common.Enums;
using System.Collections.Generic;

namespace SchoolManage.Common.Tests.Seeds;

public static class ActivitySeeds
{
	public static readonly ActivityEntity EmptyActivity = new()
	{
		Id = default,
		Start = default!,
		End = default!,
		Room = default!,
		Type = default!,
		Subjects = default!,
        SubjectsId = default!
    };
	
    public static readonly ActivityEntity one = new()
    {
        Id = Guid.NewGuid(),
        Start = DateTime.Now,
        End = DateTime.Now.AddHours(1),
        Room = Room.Auditorium,
        Type = ActivityType.Exam,
        Description = "First activity",
        Subjects = SubjectSeeds.math,
        SubjectsId = SubjectSeeds.math.Id
    };

    public static readonly ActivityEntity two = new()
    {
        Id = Guid.NewGuid(),
        Start = DateTime.Now,
        End = DateTime.Now.AddHours(1),
        Room = Room.Classroom,
        Type = ActivityType.Homework,
        Description = "Second activity",
        Subjects = SubjectSeeds.english,
        SubjectsId = SubjectSeeds.english.Id
    };

    //To ensure that no tests reuse these clones for non-idempotent operations
    public static readonly ActivityEntity ActivityEntityUpdate = one with { Id = Guid.Parse("0953F3CE-7B1A-48C1-9796-D2BAC7F67868"), Evaluations = Array.Empty<EvaluationEntity>(), Subjects = null! };
    public static readonly ActivityEntity ActivityEntityDelete = two with { Id = Guid.Parse("5DCA4CEA-B8A8-4C86-A0B3-FFB78FBA1A09"), Evaluations = Array.Empty<EvaluationEntity>(), Subjects = null! };

    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityEntity>().HasData(
            one with { Subjects = null! },
            two with { Subjects = null! },
            ActivityEntityUpdate with { Subjects = null! },
            ActivityEntityDelete with { Subjects = null! }
        );
    }
}

