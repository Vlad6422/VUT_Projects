using Time2Plan.BL.Mappers;
using Time2Plan.BL.Models;
using Time2Plan.DAL.Entities;
using Time2Plan.DAL.Mappers;
using Time2Plan.DAL.UnitOfWork;

namespace Time2Plan.BL.Facades;

public class UserFacade : FacadeBase<UserEntity, UserListModel, UserDetailModel, UserEntityMapper>, IUserFacade
{
    public UserFacade(IUnitOfWorkFactory unitOfWorkFactory, IUserModelMapper modelMapper) : base(unitOfWorkFactory, modelMapper)
    {
    }

    protected override string IncludesNavigationPathDetail 
        => $"{nameof(UserEntity.UserProjects)}.{nameof(ProjectUserRelation.Project)}";
}