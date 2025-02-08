using SchoolManage.Common.Enums;
using SchoolManage.DAL.Entities;
using System;


namespace SchoolManage.BL.Models;

public record SubjectListModel : ModelBase
{
    public required string Name { get; set; }
    public required string Acronym { get; set; }

    public static SubjectListModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        Acronym = string.Empty
    };
}
