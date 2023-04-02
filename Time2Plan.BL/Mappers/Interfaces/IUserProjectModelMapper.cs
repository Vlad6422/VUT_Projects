using System;
using Time2Plan.BL.Models;
using Time2Plan.DAL.Interfaces;

namespace Time2Plan.BL.Mappers.Interfaces;

public interface IUserProjectModelMapper
    : IModelMapper<ProjectUserRelation, UserProjectListModel, UserProjectDetailModel>
{
    UserProjectListModel MapToListModel(UserProjectDetailModel detailModel);
    ProjectUserRelation MapToEntity(UserProjectDetailModel model, Guid userId, Guid projectId);
    void MapToExistingDetailModel(UserProjectDetailModel existingDetailModel, UserListModel user, ProjectListModel project);
    ProjectUserRelation MapToEntity(UserProjectListModel model, Guid userId, Guid projectId);
}
