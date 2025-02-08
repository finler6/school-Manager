using SchoolManage.BL.Models;
using SchoolManage.DAL.Entities;
using System.Collections.ObjectModel;

namespace SchoolManage.BL.Mappers;

public class StudentModelMapper : ModelMapperBase<StudentEntity, StudentListModel, StudentDetailModel>, IStudentModelMapper
{
    public override StudentListModel MapToListModel(StudentEntity? entity)
        => entity is null
            ? StudentListModel.Empty
            : new StudentListModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Surname = entity.Surname
            };

    public override StudentDetailModel MapToDetailModel(StudentEntity? entity)
    {
        var subjectModelMapper = new SubjectModelMapper();
        var evaluationModelMapper = new EvaluationModelMapper();
        
        if (entity == null)
        {
            return StudentDetailModel.Empty;
        }

        var subjects = subjectModelMapper != null
            ? subjectModelMapper.MapToListModel(entity.Subjects).ToObservableCollection()
            : new ObservableCollection<SubjectListModel>();

        var evaluations = evaluationModelMapper != null
            ? evaluationModelMapper.MapToListModel(entity.Evaluations).ToObservableCollection()
            : new ObservableCollection<EvaluationListModel>();

        return new StudentDetailModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Surname = entity.Surname,
            ImageUrl = entity.ImageUrl,
            Subjects = subjects,
            Evaluations = evaluations
        };
    }

    public StudentListModel MapToListModel(StudentDetailModel model)
       => new()
       {
           Id = model.Id,
           Name = model.Name,
           Surname = model.Surname
       };

    public override StudentEntity MapToEntity(StudentDetailModel model)
    {
        var entity = new StudentEntity
        {
            Id = model.Id,
            Name = model.Name,
            Surname = model.Surname,
            ImageUrl = model.ImageUrl
        };

        return entity;
    }

    public StudentEntity MapToEntity(StudentListModel model)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            Surname = model.Surname,
        };
}