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
    public async Task<IEnumerable<ActivityListModel>> GetAsyncFilter(DateTime? fromDate, DateTime? toDate, string? tag, Guid? projectId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<ActivityEntity> query = uow.GetRepository<ActivityEntity, ActivityEntityMapper>().Get();


        if (fromDate != null)
        {
            query = query.Where(e => e.Start > fromDate);
        }
        if (toDate != null)
        {
            query = query.Where(e => e.Start < toDate);
        }
        if (tag != null)
        {
            query = query.Where(e => e.Tag == tag);
        }
        if (projectId != null)
        {
            query = query.Where(e => e.Project!.Id == projectId);

        }
        List<ActivityEntity> entities = await query.ToListAsync();

        return ModelMapper.MapToListModel(entities);
    }

    public async Task<IEnumerable<ActivityListModel>> GetAsyncFilter(DateTime fromDate, DateTime toDate)
    {
        return await GetAsyncFilter(fromDate, toDate, null, null);
    }

    public async Task<IEnumerable<ActivityListModel>> GetAsyncFilter(string tag)
    {
        return await GetAsyncFilter(null, null, tag, null);
    }

    public async Task<IEnumerable<ActivityListModel>> GetAsyncFilter(Guid projectId)
    {
        return await GetAsyncFilter(null, null, null, projectId);
    }


    public async Task<IEnumerable<ActivityListModel>> GetAsyncFilter(IActivityFacade.Interval interval, string? tag, Guid? projectId)
    {
        DateTime toDate = DateTime.Now;
        DateTime fromDate;

        switch (interval)
        {
            case IActivityFacade.Interval.Daily:
                fromDate = toDate.AddDays(-1);
                break;
            case IActivityFacade.Interval.Weekly:
                fromDate = toDate.AddDays(-7);
                break;
            case IActivityFacade.Interval.Monthly:
                fromDate = toDate.AddMonths(-1);
                break;
            case IActivityFacade.Interval.Yearly:
                fromDate = toDate.AddYears(-1);
                break;
            default:
                throw new Exception("Undefined interval");
        }
        return await GetAsyncFilter(fromDate, toDate, tag, projectId);
    }

    public async Task<IEnumerable<ActivityListModel>> GetAsyncFilter(IActivityFacade.Interval interval)
    {
        return await GetAsyncFilter(interval, null, null);
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