using Microsoft.EntityFrameworkCore;
using Time2Plan.BL.Mappers;
using Time2Plan.BL.Models;
using Time2Plan.DAL.Entities;
using Time2Plan.DAL.Mappers;
using Time2Plan.DAL.UnitOfWork;

namespace Time2Plan.BL.Facades;

public class ActivityFacade : FacadeBase<ActivityEntity, ActivityListModel, ActivityDetailModel, ActivityEntityMapper>, IActivityFacade
{
    public ActivityFacade(IUnitOfWorkFactory unitOfWorkFactory, IActivityModelMapper modelMapper) : base(unitOfWorkFactory, modelMapper)
    {
    }
    public async Task<IEnumerable<ActivityListModel>> GetAsyncFilter(Guid UserId, DateTime? fromDate, DateTime? toDate, string? tag, Guid? projectId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<ActivityEntity> query = uow.GetRepository<ActivityEntity, ActivityEntityMapper>().Get();

        query = query.Where(e => e.UserId == UserId);
        if (fromDate != null)
        {
            query = query.Where(e => e.Start >= fromDate);
        }
        if (toDate != null)
        {
            query = query.Where(e => e.Start <= toDate);
        }
        if (tag != null)
        {
            query = query.Where(e => e.Tag == tag);
        }
        if (projectId != null)
        {
            query = query.Where(e => e.Project!.Id == projectId);
        }
        query = query.OrderBy(e => e.Start);
        List<ActivityEntity> entities = await query.ToListAsync();

        return ModelMapper.MapToListModel(entities);
    }

    public async Task<IEnumerable<ActivityListModel>> GetAsyncListByUser(Guid Id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<ActivityEntity> entities = uow
            .GetRepository<ActivityEntity, ActivityEntityMapper>()
            .Get()
            .Where(e => e.UserId == Id) // find all activities by user
            .ToList();

        return ModelMapper.MapToListModel(entities);
    }
}