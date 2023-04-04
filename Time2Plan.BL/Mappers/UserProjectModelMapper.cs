using System;
using Time2Plan.BL.Models;
using Time2Plan.DAL.Interfaces;
using Time2Plan.BL.Mappers.Interfaces;

namespace Time2Plan.BL.Mappers;

public class UserProjectModelMapper : ModelMapperBase<ProjectUserRelation, UserProjectListModel, UserProjectDetailModel>,
    IUserProjectModelMapper
{
    public override UserProjectDetailModel MapToDetailModel(ProjectUserRelation? entity)
        => (entity?.User is null || entity?.Project is null) ? UserProjectDetailModel.Empty : new UserProjectDetailModel
        {
            Id= entity.Id,
            UserId = entity.UserId,
            ProjectId = entity.ProjectId,
            UserName = entity.User.Name,
            ProjectName = entity.Project.Name
        };

    public UserProjectListModel MapToListModel(UserProjectDetailModel detailModel)
        => new()
        {
            Id = detailModel.Id,
            UserId = detailModel.UserId,
            ProjectId = detailModel.ProjectId,
        };

    public override ProjectUserRelation MapToEntity(UserProjectDetailModel model)
         => new()
         {
             Id = model.Id,
             UserId = model.UserId,
             ProjectId = model.ProjectId
         };

    public ProjectUserRelation MapToEntity(UserProjectListModel model)
        => new()
        {
            Id = model.Id,
            UserId = model.UserId,
            ProjectId = model.ProjectId
        };

    public void MapToExistingDetailModel(UserProjectDetailModel existingDetailModel, UserListModel user, ProjectListModel project)
    {
        existingDetailModel.UserId = user.Id;
        existingDetailModel.ProjectId = project.Id;
        existingDetailModel.ProjectName = project.Name;
        existingDetailModel.UserName = user.Name;

    }

   
    public override UserProjectListModel MapToListModel(ProjectUserRelation? entity)
        => (entity?.User is null || entity?.Project is null) ? UserProjectListModel.Empty : new UserProjectListModel
        {
            Id = entity.Id,
            UserId = entity.UserId,
            ProjectId = entity.ProjectId
        };
}

