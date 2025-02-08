using SchoolManage.BL.Models;
using SchoolManage.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManage.BL.Facades;

public interface IFacade<TEntity, TListModel, TDetailModel>
    where TEntity : class, IEntity
    where TListModel : IModel
    where TDetailModel : class, IModel
{
    Task DeleteAsync(Guid id);
    Task<TDetailModel?> GetAsync(Guid id);
    Task<IEnumerable<TListModel>> GetAsync();
    Task<TDetailModel> SaveAsync(TDetailModel model);
}
