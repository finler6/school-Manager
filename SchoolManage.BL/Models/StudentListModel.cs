using SchoolManage.Common.Enums;
using SchoolManage.DAL.Entities;
using System;


namespace SchoolManage.BL.Models;

public record StudentListModel : ModelBase
{
    public required string Name { get; set; }
    public required string Surname { get; set; }

    public static StudentListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        Surname = string.Empty
    };
}
