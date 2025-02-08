using SchoolManage.BL.Models;
using SchoolManage.DAL.Entities;

namespace SchoolManage.BL.Mappers;

public interface IStudentModelMapper : IModelMapper<StudentEntity, StudentListModel, StudentDetailModel>
{
    StudentListModel MapToListModel(StudentEntity? entity);
    StudentDetailModel MapToDetailModel(StudentEntity? entity);
    StudentListModel MapToListModel(StudentDetailModel model);
    StudentEntity MapToEntity(StudentDetailModel model);
    StudentEntity MapToEntity(StudentListModel model);
}