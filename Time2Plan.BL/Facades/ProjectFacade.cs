using Time2Plan.BL.Facades.Interfaces;
using Time2Plan.BL.Mappers.Interfaces;
using Time2Plan.BL.Models;
using Time2Plan.DAL.Entities;
using Time2Plan.DAL.Mappers;
using Time2Plan.DAL.UnitOfWork;

namespace Time2Plan.BL.Facades;

public class ProjectFasade : FacadeBase<ProjectEntity, ProjectListModel, ProjectDetailModel, ProjectEntityMapper>, IProjectFacade
{
    public ProjectFasade(IUnitOfWorkFactory unitOfWorkFactory, IProjectModelMapper modelMapper) : base(unitOfWorkFactory, modelMapper)
    {
    }
}