using SchoolManage.BL.Models;
using SchoolManage.DAL.Entities;

namespace SchoolManage.BL.Mappers;

public class EvaluationModelMapper :
    ModelMapperBase<EvaluationEntity, EvaluationListModel, EvaluationDetailModel>
{
    public override EvaluationListModel MapToListModel(EvaluationEntity? entity)
        => entity is null
            ? EvaluationListModel.Empty
            : new EvaluationListModel
            {
                Id = entity.Id,
                Points = entity.Points,
                Activities = entity.Activities,
                Students = entity.Students,
            };

    public override EvaluationDetailModel MapToDetailModel(EvaluationEntity? entity)
        => entity is null
            ? EvaluationDetailModel.Empty
            : new EvaluationDetailModel
            {
                Id = entity.Id,
                Points = entity.Points,
                Description = entity.Description,
                Activities = entity.Activities,
                Students = entity.Students
            };

    public override EvaluationEntity MapToEntity(EvaluationDetailModel model)
        => new()
        {
            Id = model.Id,
            Points = model.Points,
            Description = model.Description,
            Activities = model.Activities,
            Students = model.Students,
            ActivitiesId = model.Activities.Id,
            StudentsId = model.Students.Id
        };
}