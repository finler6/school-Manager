using System.Collections.ObjectModel;
using System.Diagnostics;
using SchoolManage.BL.Mappers;
using SchoolManage.BL.Models;
using SchoolManage.DAL.Entities;
using SchoolManage.DAL.Mappers;
using SchoolManage.DAL.Repositories;
using SchoolManage.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace SchoolManage.BL.Facades;

public class SubjectFacade : FacadeBase<SubjectEntity, SubjectListModel, SubjectDetailModel, SubjectEntityMapper>, ISubjectFacade
{
    public SubjectFacade(IUnitOfWorkFactory unitOfWorkFactory, SubjectModelMapper subjectModelMapper)
        : base(unitOfWorkFactory, subjectModelMapper)
    {
    }

    public async Task CreateSubjectAsync(SubjectDetailModel model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        var entity = ModelMapper.MapToEntity(model);

        await using var uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<SubjectEntity, SubjectEntityMapper>();

        if (await repository.ExistsAsync(entity))
        {
            throw new InvalidOperationException($"Subject with ID {entity.Id} already exists.");
        }

        repository.Insert(entity);
        await uow.CommitAsync();
    }

    public async Task UpdateSubjectAsync(SubjectDetailModel model)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        await using var uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<SubjectEntity, SubjectEntityMapper>();

        if (!await repository.ExistsAsync(ModelMapper.MapToEntity(model)))
        {
            throw new KeyNotFoundException($"Subject with ID {model.Id} not found.");
        }

        var entity = ModelMapper.MapToEntity(model);
        await repository.UpdateAsync(entity);
        await uow.CommitAsync();
    }

    public async Task DeleteSubjectAsync(Guid subjectId)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<SubjectEntity, SubjectEntityMapper>();

        var subject = await repository.Get().SingleOrDefaultAsync(e => e.Id == subjectId);
        if (subject == null)
        {
            throw new KeyNotFoundException($"Subject with ID {subjectId} not found.");
        }

        await repository.DeleteAsync(subjectId);
        await uow.CommitAsync();
    }

    public async Task<IEnumerable<SubjectListModel>> GetAllSubjectsAsync()
    {
        await using var uow = UnitOfWorkFactory.Create();
        var subjectRepository = uow.GetRepository<SubjectEntity, SubjectEntityMapper>();

        var subjects = await subjectRepository
            .Get()
            .ToListAsync();

        var subjectModelMapper = new SubjectModelMapper();

        return subjects.Select(subjectModelMapper.MapToListModel).ToList();
    }

    public async Task<IEnumerable<SubjectListModel>> SearchSubjectsAsync(string searchQuery)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var subjectRepository = uow.GetRepository<SubjectEntity, SubjectEntityMapper>();

        var lowerCaseQuery = searchQuery.ToLower();

        var subjects = await subjectRepository
            .Get()
            .Where(s => s.Name.ToLower().Contains(lowerCaseQuery) || s.Acronym.ToLower().Contains(lowerCaseQuery))
            .ToListAsync();

        var subjectModelMapper = new SubjectModelMapper();

        return subjects.Select(subject => subjectModelMapper.MapToListModel(subject)).ToList();
    }
    
    public async Task <ObservableCollection<ActivityListModel>> GetSubjectActivitiesAsync(Guid id)
    {
        var activityFacade = new ActivityFacade(UnitOfWorkFactory, new ActivityModelMapper());
        
        var acts = await activityFacade.GetActivitiesBySubjectIdAsync(id);

        return acts.ToObservableCollection();
    }
    
}