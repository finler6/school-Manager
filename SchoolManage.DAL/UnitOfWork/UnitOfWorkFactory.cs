using Microsoft.EntityFrameworkCore;

namespace SchoolManage.DAL.UnitOfWork;

public class UnitOfWorkFactory(IDbContextFactory<SchoolManageDbContext> dbContextFactory) : IUnitOfWorkFactory
{
    public IUnitOfWork Create() => new UnitOfWork(dbContextFactory.CreateDbContext());
}
