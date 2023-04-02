using System;
using Time2Plan.BL.Models;
using Time2Plan.DAL.Interfaces;

namespace Time2Plan.BL.Mappers.Interfaces;

public interface IActivityModelMapper : IModelMapper<ActivityEntity, ActivityListModel, ActivityDetailModel>
{
}
