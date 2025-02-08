using SchoolManage.BL.Models;
using SchoolManage.DAL.Entities;

namespace SchoolManage.BL.Mappers;

public class ActivityModelMapper :
    ModelMapperBase<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
    public override ActivityListModel MapToListModel(ActivityEntity? entity)
        => entity is null
            ? ActivityListModel.Empty
            : new ActivityListModel
            {
                Id = entity.Id,
                Start = entity.Start,
                Description = entity.Description,
                End = entity.End,
                Room = entity.Room,
                Type = entity.Type,
                Subjects = null!
            };

    public override ActivityDetailModel MapToDetailModel(ActivityEntity? entity)
        => entity is null
            ? ActivityDetailModel.Empty
            : new ActivityDetailModel
            {
                Id = entity.Id,
                Start = entity.Start,
                End = entity.End,
                Room = entity.Room,
                Type = entity.Type,
                Description = entity.Description,
                Subjects = null!,
                SubjectsId = entity.SubjectsId,
            };

    public ActivityListModel MapToListModel(ActivityDetailModel model)
        => new()
        {
            Id = model.Id,
            Start = model.Start,
            End = model.End,
            Room = model.Room,
            Type = model.Type,
            Description = model.Description,
            Subjects = null!
        };

    public override ActivityEntity MapToEntity(ActivityDetailModel model)
    {
        return new ActivityEntity
        {
            Id = model.Id,
            Start = model.Start,
            End = model.End,
            Room = model.Room,
            Type = model.Type,
            Description = model.Description,
            SubjectsId = model.SubjectsId,
            Subjects = null!
        };
    }
    public void MapToExistingDetailModel(ActivityDetailModel existingDetailModel,
        SubjectDetailModel subject)
    {
        existingDetailModel.SubjectsId = subject.Id;
        existingDetailModel.Subjects.Id = subject.Id;
        existingDetailModel.Subjects.Name = subject.Name;
        existingDetailModel.Subjects.Acronym = subject.Acronym;
    }
    public ActivityEntity MapToEntity(ActivityDetailModel model, Guid SubjectsId)
        => new()
        {
            Id = model.Id,
            Start = model.Start,
            End = model.End,
            Room = model.Room,
            Type = model.Type,
            Description = model.Description,
            SubjectsId = SubjectsId,
            Subjects = null!
        };

    public ActivityEntity MapToEntity(ActivityListModel model, Guid SubjectsId)
        => new()
        {
            Id = model.Id,
            Start = model.Start,
            End = model.End,
            Room = model.Room,
            Type = model.Type,
            Description = model.Description,
            SubjectsId = SubjectsId,
            Subjects = null!
        };
}