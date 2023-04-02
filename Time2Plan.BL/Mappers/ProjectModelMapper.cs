﻿using Time2Plan.BL.Models;
using Time2Plan.DAL.Interfaces;
using Time2Plan.BL.Mappers.Interfaces;
using System.Collections.ObjectModel;

namespace Time2Plan.BL.Mappers;

public class ProjectModelMapper : ModelMapperBase<ProjectEntity, ProjectListModel, ProjectDetailModel>, IProjectModelMapper
{
    private readonly IUserProjectModelMapper _userProjectModelMapper;
    private readonly IActivityModelMapper _activityModelMapper;

    public ProjectModelMapper(IUserProjectModelMapper userProjectModelMapper) =>
        _userProjectModelMapper = userProjectModelMapper;

    public ProjectModelMapper(IActivityModelMapper activityModelMapper) =>
        _activityModelMapper = activityModelMapper;

    public override ProjectListModel MapToListModel(ProjectEntity? entity)
        => entity is null
            ? ProjectListModel.Empty
            : new ProjectListModel
            {
                Id = entity.Id,
                Name = entity.Name,
            };

    public override ProjectDetailModel MapToDetailModel(ProjectEntity? entity)
        => entity is null
            ? ProjectDetailModel.Empty
            : new ProjectDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                UserProjects = new ObservableCollection<UserProjectListModel>(_userProjectModelMapper.MapToListModel(entity.UserProjects)),   
                Activities = new ObservableCollection<ActivityListModel>(_activityModelMapper.MapToListModel(entity.Activities))
            };

    public override ProjectEntity MapToEntity(ProjectDetailModel model)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
        };
}