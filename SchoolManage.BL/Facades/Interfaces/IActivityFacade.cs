using SchoolManage.BL.Models;
using SchoolManage.DAL.Entities;


namespace SchoolManage.BL.Facades;

public interface IActivityFacade : IFacade<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
    Task DeleteActivityAsync(Guid activityId);
    Task UpdateEvaluationsAsync(Guid activityId, ICollection<EvaluationEntity> newEvaluations);
    Task UpdateActivityAsync(ActivityDetailModel model);
    Task SaveAsync(ActivityDetailModel model, Guid SubjectsId);
    Task SaveAsync(ActivityListModel model, Guid SubjectsId);
    Task<IEnumerable<ActivityListModel>> GetScheduleByDateAsync(DateTime date);
    Task<IEnumerable<ActivityListModel>> GetActivitiesBySubjectIdAsync(Guid subjectId);

    Task<IEnumerable<ActivityListModel>> GetActivitiesBetweenDatesBySubject(Guid subjectId, DateTime start,
        DateTime end);
}