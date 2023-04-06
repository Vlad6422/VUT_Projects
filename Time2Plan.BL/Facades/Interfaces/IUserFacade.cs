using Time2Plan.BL.Models;
using Time2Plan.DAL.Interfaces;

namespace Time2Plan.BL.Facades.Interfaces
{
    internal interface IUserFacade : IFacade<UserEntity, UserListModel, UserDetailModel>
    {
    }
}