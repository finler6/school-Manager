using SchoolManage.DAL.Entities;
using SchoolManage.Common.Enums;
using System;
using System.Collections.Generic;

namespace SchoolManage.BL.Models;

public record ActivityDetailModel : ModelBase
{
    public required DateTime Start { get; set; }
    public required DateTime End { get; set; }
    public required Room Room { get; set; }
    public required ActivityType Type { get; set; }

    public required string? Description { get; set; }

    public required SubjectEntity Subjects { get; init; }
    
    public ICollection<EvaluationEntity> Evaluations { get; init; } = new List<EvaluationEntity>();

    public required Guid SubjectsId { get; set; }

    public static ActivityDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Start = DateTime.MinValue, 
        End = DateTime.MinValue,   
        Room = Room.None,        
        Type = ActivityType.None, 
        Description = "", 
        Subjects = new SubjectEntity
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Acronym = string.Empty,
        },
        SubjectsId = Guid.NewGuid(),
    };
}