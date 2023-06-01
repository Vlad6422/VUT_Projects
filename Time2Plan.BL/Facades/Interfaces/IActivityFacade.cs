using Time2Plan.BL.Models;
using Time2Plan.DAL.Entities;

namespace Time2Plan.BL.Facades;

public interface IActivityFacade : IFacade<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
    Task<IEnumerable<ActivityListModel>> GetAsyncFilter(Guid UserId, DateTime? fromDate, DateTime? toDate, string? tag, Guid? projectId);

    Task<IEnumerable<ActivityListModel>> GetAsyncListByUser(Guid userId);

    enum Interval
    {
        Manual,
        All,
        Daily,
        Weekly,
        This_Month,
        Last_Month,
        Yearly
    }
}