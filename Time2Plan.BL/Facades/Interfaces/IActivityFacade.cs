using Time2Plan.BL.Models;
using Time2Plan.DAL.Entities;
using static Time2Plan.BL.Facades.ActivityFacade;

namespace Time2Plan.BL.Facades.Interfaces;

public interface IActivityFacade : IFacade<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
    Task<IEnumerable<ActivityListModel>> GetAsyncFilter(DateTime? fromDate, DateTime? toDate, string? tag, ProjectEntity? project);

    Task<IEnumerable<ActivityListModel>> GetAsyncFilter(DateTime fromDate, DateTime toDate);

    Task<IEnumerable<ActivityListModel>> GetAsyncFilter(string tag);

    Task<IEnumerable<ActivityListModel>> GetAsyncFilter(ProjectEntity project);

    Task<IEnumerable<ActivityListModel>> GetAsyncFilter(Interval interval, string? tag, ProjectEntity? project);

    Task<IEnumerable<ActivityListModel>> GetAsyncFilter(Interval interval);
}