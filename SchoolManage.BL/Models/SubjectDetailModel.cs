using SchoolManage.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace SchoolManage.BL.Models;

public record SubjectDetailModel : ModelBase
{
    public required string Name { get; set; }
    public required string Acronym { get; set; }

    public ObservableCollection<StudentListModel> Students { get; init; } = new();
    public ObservableCollection<ActivityListModel> Activities { get; init; } = new();
    public static SubjectDetailModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        Acronym = string.Empty
    };

}