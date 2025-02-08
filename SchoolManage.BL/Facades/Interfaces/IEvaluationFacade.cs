using SchoolManage.BL.Models;
using SchoolManage.DAL.Entities;

namespace SchoolManage.BL.Facades;

public interface IEvaluationFacade : IFacade<EvaluationEntity, EvaluationListModel, EvaluationDetailModel>
{
    Task CreateEvaluationAsync(EvaluationDetailModel model);
    Task UpdateEvaluationAsync(EvaluationDetailModel model);
    Task DeleteEvaluationAsync(Guid evaluationId);
    Task<IEnumerable<EvaluationListModel>> GetAllEvaluationsWithActivityIdAsync(Guid activityId);
}