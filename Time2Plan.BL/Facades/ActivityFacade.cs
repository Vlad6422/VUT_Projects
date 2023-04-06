using Time2Plan.BL.Facades.Interfaces;
using Time2Plan.BL.Mappers;
using Time2Plan.BL.Mappers.Interfaces;
using Time2Plan.BL.Models;
using Time2Plan.DAL.Interfaces;
using Time2Plan.DAL.Mappers;
using Time2Plan.DAL.UnitOfWork;

namespace Time2Plan.BL.Facades;

public class ActivityFacade : FacadeBase<ActivityEntity, ActivityListModel, ActivityDetailModel, ActivityEntityMapper>, IActivityFacade
{
    public ActivityFacade(IUnitOfWorkFactory unitOfWorkFactory,IActivityModelMapper modelMapper) : base(unitOfWorkFactory, modelMapper)
    {
    }
}