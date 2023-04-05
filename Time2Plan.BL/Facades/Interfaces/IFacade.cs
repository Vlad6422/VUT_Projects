using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time2Plan.BL.Models;
using Time2Plan.DAL.Interfaces;

namespace Time2Plan.BL.Facades.Interfaces
{
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
}
