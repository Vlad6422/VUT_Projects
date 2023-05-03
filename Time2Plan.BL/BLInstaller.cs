using Microsoft.Extensions.DependencyInjection;
using Time2Plan.BL.Facades.Interfaces;
using Time2Plan.BL.Mappers;
using Time2Plan.DAL.UnitOfWork;

namespace Time2Plan.BL;

public static class BLInstaller
{
    public static IServiceCollection AddBLServices(this IServiceCollection services)
    {
        services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();

        services.Scan(selector => selector
            .FromAssemblyOf<BusinessLayer>()
            .AddClasses(filter => filter.AssignableTo(typeof(IFacade<,,>)))
            .AsMatchingInterface()
            .WithSingletonLifetime());

        services.Scan(selector => selector
            .FromAssemblyOf<BusinessLayer>()
            .AddClasses(filter => filter.AssignableTo(typeof(ModelMapperBase<,,>)))
            .AsMatchingInterface()
            .WithSingletonLifetime());

        return services;
    }
}
