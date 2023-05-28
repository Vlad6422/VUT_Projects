using Time2Plan.BL.Models;
using Time2Plan.DAL.Entities;
using static Time2Plan.BL.Facades.ActivityFacade;

namespace Time2Plan.BL.Facades;

public interface IActivityFacade : IFacade<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
    Task<IEnumerable<ActivityListModel>> GetAsyncFilter(DateTime? fromDate, DateTime? toDate, string? tag, Guid? projectId);

    Task<IEnumerable<ActivityListModel>> GetAsyncFilter(DateTime fromDate, DateTime toDate);

    Task<IEnumerable<ActivityListModel>> GetAsyncFilter(string tag);

    Task<IEnumerable<ActivityListModel>> GetAsyncFilter(Guid projectId);

    Task<IEnumerable<ActivityListModel>> GetAsyncFilter(Interval interval, string? tag, Guid? projectId);

    Task<IEnumerable<ActivityListModel>> GetAsyncFilter(Interval interval);

    Task<IEnumerable<ActivityListModel>> GetAsyncListByUser(Guid userId);

    enum Interval
    {
        Daily,
        Weekly,
        Monthly,
        Yearly,
        All
    }
}