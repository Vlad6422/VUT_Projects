using Time2Plan.BL.Models;
using Time2Plan.DAL.Entities;

namespace Time2Plan.BL.Mappers;

public class ActivityModelMapper : ModelMapperBase<ActivityEntity, ActivityListModel, ActivityDetailModel>,
    IActivityModelMapper
{
    public override ActivityListModel MapToListModel(ActivityEntity? entity)
        => entity is null
            ? ActivityListModel.Empty
            : new ActivityListModel
            {
                Id = entity.Id,
                Start = entity.Start,
                End = entity.End,
                Type = entity.Type,
                Tag = entity.Tag,
                UserId = entity.UserId,
                ProjectId = entity.ProjectId,
            };

    public override ActivityDetailModel MapToDetailModel(ActivityEntity? entity)
        => entity is null
            ? ActivityDetailModel.Empty
            : new ActivityDetailModel
            {
                Id = entity.Id,
                Start = entity.Start,
                End = entity.End,
                Type = entity.Type,
                Tag = entity.Tag,
                Description = entity.Description,
                UserId = entity.UserId,
                ProjectId = entity.ProjectId,
            };

    public override ActivityEntity MapToEntity(ActivityDetailModel model)
        => new()
        {
            Id = model.Id,
            Start = model.Start,
            End = model.End,
            Type = model.Type,
            Tag = model.Tag,
            Description = model.Description,
            UserId = model.UserId,
            ProjectId = model.ProjectId,
        };
}