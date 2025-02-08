using SchoolManage.DAL.Entities;
using SchoolManage.DAL.Mappers;
using SchoolManage.DAL.Repositories;

namespace SchoolManage.DAL.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<TEntity> GetRepository<TEntity, TEntityMapper>()
        where TEntity : class, IEntity
        where TEntityMapper : IEntityMapper<TEntity>, new();

    Task CommitAsync();
}
