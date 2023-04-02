using System;
using Time2Plan.BL.Models;
using Time2Plan.DAL.Interfaces;
using Time2Plan.BL.Mappers.Interfaces;

namespace Time2Plan.BL.Mappers;

public class UserProjectModelMapper : ModelMapperBase<ProjectUserRelation, UserProjectListModel, UserProjectDetailModel>,
    IUserProjectModelMapper
{
    public override UserProjectDetailModel MapToDetailModel(ProjectUserRelation entity)
    {
        throw new NotImplementedException();
    }

    public ProjectUserRelation MapToEntity(UserProjectDetailModel model, Guid userId, Guid projectId)
    {
        throw new NotImplementedException();
    }

    public ProjectUserRelation MapToEntity(UserProjectListModel model, Guid userId, Guid projectId)
    {
        throw new NotImplementedException();
    }

    public override ProjectUserRelation MapToEntity(UserProjectDetailModel model)
    {
        throw new NotImplementedException();
    }

    public void MapToExistingDetailModel(UserProjectDetailModel existingDetailModel, UserListModel user, ProjectListModel project)
    {
        throw new NotImplementedException();
    }

    public UserProjectListModel MapToListModel(UserProjectDetailModel detailModel)
    {
        throw new NotImplementedException();
    }

    public override UserProjectListModel MapToListModel(ProjectUserRelation? entity)
    {
        throw new NotImplementedException();
    }
    //{
    //    public override UserProjectListModel MapToListModel(ProjectUserRelation? entity)
    //        => entity?.Project is null
    //            ? UserProjectListModel.Empty
    //            : new UserProjectListModel
    //            {
    //                Id = entity.Id,
    //                IngredientId = entity.Ingredient.Id,
    //                IngredientName = entity.Ingredient.Name,
    //                IngredientImageUrl = entity.Ingredient.ImageUrl,
    //                Amount = entity.Amount,
    //                Unit = entity.Unit
    //            };

    //    public override UserProjectDetailModel MapToDetailModel(ProjectUserRelation? entity)
    //        => entity?.Ingredient is null
    //            ? UserProjectDetailModel.Empty
    //            : new UserProjectDetailModel
    //            {
    //                Id = entity.Id,
    //                IngredientId = entity.Ingredient.Id,
    //                IngredientName = entity.Ingredient.Name,
    //                IngredientDescription = entity.Ingredient.Description,
    //                IngredientImageUrl = entity.Ingredient.ImageUrl,
    //                Amount = entity.Amount,
    //                Unit = entity.Unit
    //            };

    //    public UserProjectListModel MapToListModel(UserProjectDetailModel detailModel)
    //        => new()
    //        {
    //            Id = detailModel.Id,
    //            IngredientId = detailModel.IngredientId,
    //            IngredientName = detailModel.IngredientName,
    //            IngredientImageUrl = detailModel.IngredientImageUrl,
    //            Amount = detailModel.Amount,
    //            Unit = detailModel.Unit
    //        };

    //    public void MapToExistingDetailModel(UserProjectDetailModel existingDetailModel,
    //        IngredientListModel ingredient)
    //    {
    //        existingDetailModel.IngredientId = ingredient.Id;
    //        existingDetailModel.IngredientName = ingredient.Name;
    //        existingDetailModel.IngredientImageUrl = ingredient.ImageUrl;
    //    }

    //    public override ProjectUserRelation MapToEntity(UserProjectDetailModel model)
    //        => throw new NotImplementedException("This method is unsupported. Use the other overload.");


    //    public ProjectUserRelation MapToEntity(UserProjectDetailModel model, Guid recipeId)
    //        => new()
    //        {
    //            Id = model.Id,
    //            RecipeId = recipeId,
    //            IngredientId = model.IngredientId,
    //            Amount = model.Amount,
    //            Unit = model.Unit
    //        };

    //    public ProjectUserRelation MapToEntity(UserProjectListModel model, Guid recipeId)
    //        => new()
    //        {
    //            Id = model.Id,
    //            RecipeId = recipeId,
    //            IngredientId = model.IngredientId,
    //            Amount = model.Amount,
    //            Unit = model.Unit
    //        };
}
