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
    public virtual async Task<IEnumerable<ActivityListModel>> GetAsyncFilter(DateTime? fromDate, DateTime? toDate, string? tag, ProjectEntity? project)
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
        if (project != null)
        {
            query = query.Where(e => e.Project == project);

        }
        List<ActivityEntity> entities = await query.ToListAsync();

        return ModelMapper.MapToListModel(entities);
    }

    public virtual async Task<IEnumerable<ActivityListModel>> GetAsyncFilter(DateTime fromDate, DateTime toDate)
    {
        return await GetAsyncFilter(fromDate, toDate, null, null);
    }

    public virtual async Task<IEnumerable<ActivityListModel>> GetAsyncFilter(string tag)
    {
        return await GetAsyncFilter(null, null, tag, null);
    }

    public virtual async Task<IEnumerable<ActivityListModel>> GetAsyncFilter(ProjectEntity project)
    {
        return await GetAsyncFilter(null, null, null, project);
    }


    public virtual async Task<IEnumerable<ActivityListModel>> GetAsyncFilter(Interval interval, string? tag, ProjectEntity? project)
    {
        DateTime toDate = DateTime.Now;
        DateTime fromDate;

        switch (interval)
        {
            case Interval.Daily:
                fromDate = toDate.AddDays(-1);
                break;
            case Interval.Weekly:
                fromDate = toDate.AddDays(-7);
                break;
            case Interval.Monthly:
                fromDate = toDate.AddMonths(-1);
                break;
            case Interval.Yearly:
                fromDate = toDate.AddYears(-1);
                break;
            default:
                throw new Exception("Undefined interval");
        }
        return await GetAsyncFilter(fromDate, toDate, tag, project);
    }

    public virtual async Task<IEnumerable<ActivityListModel>> GetAsyncFilter(Interval interval)
    {
        return await GetAsyncFilter(interval, null, null);
    }
    public enum Interval
    {
        Daily,
        Weekly,
        Monthly,
        Yearly,
        All
    }
}