using SchoolManage.BL.Mappers;
using SchoolManage.BL.Models;
using SchoolManage.DAL.Entities;
using SchoolManage.DAL.Mappers;
using SchoolManage.DAL.Repositories;
using SchoolManage.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace SchoolManage.BL.Facades;

public class EvaluationFacade (
    IUnitOfWorkFactory unitOfWorkFactory,
    EvaluationModelMapper evaluationModelMapper) : FacadeBase<EvaluationEntity, EvaluationListModel, EvaluationDetailModel, EvaluationEntityMapper>(unitOfWorkFactory, evaluationModelMapper), IEvaluationFacade
{

    public async Task CreateEvaluationAsync(EvaluationDetailModel model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        var entity = ModelMapper.MapToEntity(model);

        await using var uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<EvaluationEntity, EvaluationEntityMapper>();

        if (await repository.ExistsAsync(entity))
        {
            throw new InvalidOperationException($"Evaluation with ID {entity.Id} already exists.");
        }

        repository.Insert(entity);
        await uow.CommitAsync();
    }

    public async Task UpdateEvaluationAsync(EvaluationDetailModel model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        await using var uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<EvaluationEntity, EvaluationEntityMapper>();

        var existingEvaluation = await repository.Get().SingleOrDefaultAsync(e => e.Id == model.Id);
        if (existingEvaluation == null)
        {
            CreateEvaluationAsync(model);
            return;
        }
        
        existingEvaluation = ModelMapper.MapToEntity(model);
        await repository.UpdateAsync(existingEvaluation);
        await uow.CommitAsync();
    }

    public async Task DeleteEvaluationAsync(Guid evaluationId)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<EvaluationEntity, EvaluationEntityMapper>();

        var evaluation = await repository.Get().SingleOrDefaultAsync(e => e.Id == evaluationId);
        if (evaluation == null)
        {
            throw new KeyNotFoundException($"Evaluation with ID {evaluationId} not found.");
        }

        await repository.DeleteAsync(evaluationId);
        await uow.CommitAsync();
    }
    public async Task<IEnumerable<EvaluationListModel>> GetAllEvaluationsWithActivityIdAsync(Guid activityId)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<EvaluationEntity, EvaluationEntityMapper>();

        var evaluations = await repository
            .Get()
            .Include(e => e.Activities)
            .Include(e => e.Students)
            .Where(e => e.Activities.Id == activityId)
            .ToListAsync();

        var evaluationModelMapper = new EvaluationModelMapper();

        return evaluations.Select(evaluationModelMapper.MapToListModel).ToList();
    }
};