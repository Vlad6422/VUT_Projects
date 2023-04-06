using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Time2Plan.BL.Facades.Interfaces;
using Time2Plan.BL.Mappers;
using Time2Plan.BL.Mappers.Interfaces;
using Time2Plan.BL.Models;
using Time2Plan.DAL.Interfaces;
using Time2Plan.DAL.Mappers;
using Time2Plan.DAL.UnitOfWork;

namespace Time2Plan.BL.Facades;

public class ActivityFacade : FacadeBase<ActivityEntity, ActivityListModel, ActivityDetailModel, ActivityEntityMapper>, IActivityFacade
{
    public ActivityFacade(IUnitOfWorkFactory unitOfWorkFactory,IActivityModelMapper modelMapper) : base(unitOfWorkFactory, modelMapper)
    {
    }
    public virtual async Task<IEnumerable<ActivityListModel>> GetAsyncFilter(ActivityListModel model, Guid projectId, DateTime fromDate, DateTime toDate, string tag, ProjectEntity project)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<ActivityEntity> query = uow.GetRepository<ActivityEntity, ActivityEntityMapper>().Get();

        List<ActivityEntity> entities = await query
            .Where(e => e.Start > fromDate)
            .Where(e => e.Start < toDate)
            .Where (e => e.Project == project)
            .Where(e => e.Tag == tag)
            .ToListAsync();

        return ModelMapper.MapToListModel(entities);
    }

}