using Time2Plan.BL.Models;
using Time2Plan.DAL.Entities;
using static Time2Plan.BL.Facades.ActivityFacade;

namespace Time2Plan.BL.Facades;

public interface IActivityFacade : IFacade<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
    Task<IEnumerable<ActivityListModel>> GetAsyncFilter(Guid UserId, DateTime? fromDate, DateTime? toDate, string? tag, Guid? projectId);

    Task<IEnumerable<ActivityListModel>> GetAsyncListByUser(Guid userId);

    enum Interval
    {
        All,
        Daily,
        Weekly,
        Monthly,
        Yearly,
        Manual
    }
}