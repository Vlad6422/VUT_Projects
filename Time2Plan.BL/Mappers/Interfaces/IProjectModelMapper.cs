using Time2Plan.BL.Models;
using Time2Plan.DAL.Entities;

namespace Time2Plan.BL.Mappers;

public interface IProjectModelMapper : IModelMapper<ProjectEntity, ProjectListModel, ProjectDetailModel>
{
}
