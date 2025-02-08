using System.Collections.ObjectModel;
using SchoolManage.BL.Models;
using SchoolManage.DAL.Entities;

namespace SchoolManage.BL.Facades;

public interface ISubjectFacade : IFacade<SubjectEntity, SubjectListModel, SubjectDetailModel>
{
    Task CreateSubjectAsync(SubjectDetailModel model);
    Task UpdateSubjectAsync(SubjectDetailModel model);
    Task DeleteSubjectAsync(Guid subjectId);
    Task<IEnumerable<SubjectListModel>> GetAllSubjectsAsync();
    Task<IEnumerable<SubjectListModel>> SearchSubjectsAsync(string searchQuery);
    Task<ObservableCollection<ActivityListModel>> GetSubjectActivitiesAsync(Guid id);
}