using Time2Plan.BL.Models;
using Time2Plan.DAL.Entities;

namespace Time2Plan.BL.Mappers;

public interface IUserProjectModelMapper
    : IModelMapper<ProjectUserRelation, UserProjectListModel, UserProjectDetailModel>
{
    UserProjectListModel MapToListModel(UserProjectDetailModel detailModel);
    ProjectUserRelation MapToEntity(UserProjectListModel model);
    void MapToExistingDetailModel(UserProjectDetailModel existingDetailModel, UserListModel user, ProjectListModel project);
    new ProjectUserRelation MapToEntity(UserProjectDetailModel model);

}
