using Time2Plan.BL.Facades.Interfaces;
using Time2Plan.BL.Mappers.Interfaces;
using Time2Plan.BL.Models;
using Time2Plan.DAL.Entities;
using Time2Plan.DAL.Mappers;
using Time2Plan.DAL.UnitOfWork;

namespace Time2Plan.BL.Facades;

public class UserFasade : FacadeBase<UserEntity, UserListModel, UserDetailModel, UserEntityMapper>, IUserFacade
{
    public UserFasade(IUnitOfWorkFactory unitOfWorkFactory, IUserModelMapper modelMapper) : base(unitOfWorkFactory, modelMapper)
    {
    }
}