using SchoolManage.DAL.Entities;
using System;

namespace SchoolManage.BL.Models;

public record EvaluationListModel : ModelBase
{
    public required string Points { get; set; }
    public required ActivityEntity Activities { get; init; }
    public required StudentEntity Students { get; init; }


    public static EvaluationListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Points = string.Empty,
        Activities = new ActivityEntity
        {
            Start = DateTime.Now,
            End = DateTime.Now,
            Room = 0,
            Type = Common.Enums.ActivityType.None,
            Subjects = new SubjectEntity
            {
                Id = Guid.NewGuid(),
                Name = string.Empty,
                Acronym = string.Empty,
            },
            SubjectsId = Guid.Empty,
            Id = Guid.Empty,
        },
        Students = new StudentEntity
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Surname = string.Empty,
        },
    };

}