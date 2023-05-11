using Time2Plan.BL.Facades;
using Time2Plan.BL.Mappers;
using Time2Plan.BL.Models;
using Time2Plan.DAL.Entities;
using Time2Plan.DAL.Mappers;
using Time2Plan.DAL.UnitOfWork;

namespace Time2Plan.BL.Facades;

public class ProjectFacade : FacadeBase<ProjectEntity, ProjectListModel, ProjectDetailModel, ProjectEntityMapper>, IProjectFacade
{
    public ProjectFacade(IUnitOfWorkFactory unitOfWorkFactory, IProjectModelMapper modelMapper) : base(unitOfWorkFactory, modelMapper)
    {
    }
}