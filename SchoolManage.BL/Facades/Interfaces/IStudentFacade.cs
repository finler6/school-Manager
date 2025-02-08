using SchoolManage.BL.Models;
using SchoolManage.DAL.Entities;

namespace SchoolManage.BL.Facades;

public interface IStudentFacade : IFacade<StudentEntity, StudentListModel, StudentDetailModel>
{
    Task CreateStudentAsync(StudentDetailModel model);
    Task UpdateStudentAsync(StudentDetailModel model);
    Task DeleteStudentAsync(Guid studentId);
    Task<IEnumerable<SubjectListModel>> GetStudentSubjectsAsync(Guid studentId);
    Task<IEnumerable<EvaluationListModel>> GetStudentEvaluationsAsync(Guid studentId);
    Task<IEnumerable<StudentListModel>> GetAllStudentsAsync();
    Task<IEnumerable<StudentListModel>> GetAllStudentsWithSubjectAsync(SubjectEntity subject);
    Task<IEnumerable<StudentListModel>> SearchStudentsAsync(string searchQuery);
}