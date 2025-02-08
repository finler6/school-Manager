using System.Runtime.InteropServices.JavaScript;
using SchoolManage.BL.Mappers;
using SchoolManage.BL.Models;
using SchoolManage.DAL.Entities;
using SchoolManage.DAL.Mappers;
using SchoolManage.DAL.Repositories;
using SchoolManage.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;


namespace SchoolManage.BL.Facades;

public class ActivityFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    ActivityModelMapper activityModelMapper)
    :
        FacadeBase<ActivityEntity, ActivityListModel, ActivityDetailModel,
            ActivityEntityMapper>(unitOfWorkFactory, activityModelMapper), IActivityFacade
{
    public async Task SaveAsync(ActivityListModel model, Guid SubjectsId)
    {
        ActivityEntity entity = activityModelMapper.MapToEntity(model, SubjectsId);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<ActivityEntity> repository =
            uow.GetRepository<ActivityEntity, ActivityEntityMapper>();

        if (await repository.ExistsAsync(entity))
        {
            await repository.UpdateAsync(entity);
            await uow.CommitAsync();
        }

    }

    public async Task SaveAsync(ActivityDetailModel model, Guid SubjectsId)
    {
        ActivityEntity entity = activityModelMapper.MapToEntity(model, SubjectsId);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<ActivityEntity> repository =
            uow.GetRepository<ActivityEntity, ActivityEntityMapper>();

        repository.Insert(entity);
        await uow.CommitAsync();
    }

    public async Task UpdateActivityAsync(ActivityDetailModel model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        await using var uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<ActivityEntity, ActivityEntityMapper>();

        var existingActivity = await repository.Get().SingleOrDefaultAsync(a => a.Id == model.Id);
        if (existingActivity == null)
        {
            throw new KeyNotFoundException($"Activity with ID {model.Id} not found.");
        }

        existingActivity = activityModelMapper.MapToEntity(model, existingActivity.SubjectsId);
        await repository.UpdateAsync(existingActivity);
        await uow.CommitAsync();
    }

    public async Task DeleteActivityAsync(Guid activityId)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<ActivityEntity, ActivityEntityMapper>();

        var activity = await repository.Get().SingleOrDefaultAsync(a => a.Id == activityId);
        if (activity == null)
        {
            throw new KeyNotFoundException($"Activity with ID {activityId} not found.");
        }

        await repository.DeleteAsync(activityId);
        await uow.CommitAsync();
    }

    public async Task UpdateEvaluationsAsync(Guid activityId, ICollection<EvaluationEntity> newEvaluations)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var activityRepository = uow.GetRepository<ActivityEntity, ActivityEntityMapper>();
        var evaluationRepository = uow.GetRepository<EvaluationEntity, EvaluationEntityMapper>();

        var activity = await activityRepository.Get().SingleOrDefaultAsync(a => a.Id == activityId);
        if (activity == null)
        {
            throw new KeyNotFoundException("Activity not found.");
        }

        activity.Evaluations.Clear();
        foreach (var evaluation in newEvaluations)
        {
            activity.Evaluations.Add(evaluation);
        }
        await activityRepository.UpdateAsync(activity);
        await uow.CommitAsync();
    }

    public async Task<IEnumerable<ActivityListModel>> GetScheduleByDateAsync(DateTime date)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var activityRepository = uow.GetRepository<ActivityEntity, ActivityEntityMapper>();
        var activities = await activityRepository
            .Get()
            .Where(a => a.Start.Date == date.Date)
            .OrderBy(a => a.Start)
            .ToListAsync();

        var activityModelMapper = new ActivityModelMapper();
        return activities.Select(activityModelMapper.MapToListModel).ToList();
    }
    
    public async Task<IEnumerable<ActivityListModel>> GetActivitiesBySubjectIdAsync(Guid subjectId)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var activityRepository = uow.GetRepository<ActivityEntity, ActivityEntityMapper>();

        var activities = await activityRepository
            .Get()
            .Where(a => a.SubjectsId == subjectId)
            .ToListAsync();

        var activityModelMapper = new ActivityModelMapper();
        return activities.Select(activityModelMapper.MapToListModel).ToList();
    }

    // Netestovano
    public async Task<IEnumerable<ActivityListModel>> GetActivitiesBetweenDatesBySubject(Guid subjectId, DateTime start, DateTime end)
    {
        var activities = await GetActivitiesBySubjectIdAsync(subjectId);
        var filteredActivities = activities.Where(a => a.Start >= start && a.End <= end);
        
        return filteredActivities;
    }
}