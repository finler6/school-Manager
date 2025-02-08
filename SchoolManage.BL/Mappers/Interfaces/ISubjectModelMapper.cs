using SchoolManage.BL.Models;
using SchoolManage.DAL.Entities;

namespace SchoolManage.BL.Mappers;

public interface ISubjectModelMapper : IModelMapper<SubjectEntity, SubjectListModel, SubjectDetailModel>
{
    SubjectListModel MapToListModel(SubjectEntity? entity);
    SubjectDetailModel MapToDetailModel(SubjectEntity? entity);
    SubjectListModel MapToListModel(SubjectDetailModel model);
    SubjectEntity MapToEntity(SubjectListModel model);
    SubjectEntity MapToEntity(SubjectDetailModel model);
}