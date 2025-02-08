using SchoolManage.Common.Enums;
using SchoolManage.DAL.Entities;
using System;

namespace SchoolManage.BL.Models;

public record ActivityListModel : ModelBase
{
    public required DateTime Start { get; set; }
    public required DateTime End { get; set; }
    public required Room Room { get; set; }
    public required ActivityType Type { get; set; }

    public required string? Description { get; set; }
    public required SubjectEntity Subjects { get; init; }


    public static ActivityListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Start = DateTime.MinValue,
        End = DateTime.MinValue,
        Room = new Room(),
        Description = string.Empty,
        Type = ActivityType.None,
        Subjects = new SubjectEntity
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Acronym = string.Empty,
        },
    };
}