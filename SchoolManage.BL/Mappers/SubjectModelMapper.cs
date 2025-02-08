using SchoolManage.BL.Models;
using SchoolManage.DAL.Entities;
using System.Collections.ObjectModel;

namespace SchoolManage.BL.Mappers;

public class SubjectModelMapper : ModelMapperBase<SubjectEntity, SubjectListModel, SubjectDetailModel>, ISubjectModelMapper
{
    public override SubjectListModel MapToListModel(SubjectEntity? entity)
        => entity is null
            ? SubjectListModel.Empty
            : new SubjectListModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Acronym = entity.Acronym
            };

    public override SubjectDetailModel MapToDetailModel(SubjectEntity entity)
    {
        var studentModelMapper = new StudentModelMapper();
        var activityModelMapper = new ActivityModelMapper();
        
        if (entity == null)
        {
            return SubjectDetailModel.Empty;
        }

        return new SubjectDetailModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Acronym = entity.Acronym,
            Students = studentModelMapper.MapToListModel(entity.Students).ToObservableCollection(),
            Activities = activityModelMapper.MapToListModel(entity.Activities).ToObservableCollection()
        };
    }

    public SubjectListModel MapToListModel(SubjectDetailModel model)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            Acronym = model.Acronym,
        };

    public override SubjectEntity MapToEntity(SubjectDetailModel model)
       => new()
       {
           Id = model.Id,
           Name = model.Name,
           Acronym = model.Acronym,
       };

    public SubjectEntity MapToEntity(SubjectListModel model)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            Acronym = model.Acronym,
        };
}