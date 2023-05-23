using Time2Plan.BL.Models;
using Time2Plan.DAL.Entities;

namespace Time2Plan.BL.Facades;

public interface IUserProjectFacade : IFacade<ProjectUserRelation, UserProjectListModel, UserProjectDetailModel>
{
}