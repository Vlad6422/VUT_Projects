using Time2Plan.BL.Mappers;
using Time2Plan.BL.Models;
using Time2Plan.DAL.Entities;
using Time2Plan.DAL.Mappers;
using Time2Plan.DAL.UnitOfWork;

namespace Time2Plan.BL.Facades;

public class UserProjectFacade : FacadeBase<ProjectUserRelation, UserProjectListModel, UserProjectDetailModel, ProjectUserRelationMapper>, IUserProjectFacade
{
    public UserProjectFacade(IUnitOfWorkFactory unitOfWorkFactory, IUserProjectModelMapper modelMapper) : base(unitOfWorkFactory, modelMapper)
    {
    }
}