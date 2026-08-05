using System;
using System.Threading.Tasks;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace VumbaSoft.ErmanApp.Demographics.Regions;

public class RegionManager : DomainService
{
    private readonly IRegionRepository _regionRepository;
    private readonly ISubcontinentRepository _subcontinentRepository;

    public RegionManager(
        IRegionRepository regionRepository,
        ISubcontinentRepository subcontinentRepository)
    {
        _regionRepository = regionRepository;
        _subcontinentRepository = subcontinentRepository;
    }

    public async Task<Region> CreateAsync(
        Guid subcontinentId, string name, long population = 0, string? remarks = null)
    {
        var subcontinent = await _subcontinentRepository.FindAsync(subcontinentId);
        if (subcontinent == null)
        {
            throw new UserFriendlyException($"Subcontinent with id '{subcontinentId}' was not found!");
        }

        if (await _regionRepository.FindByNameAsync(name) != null)
        {
            throw new UserFriendlyException($"A region with name '{name}' already exists!");
        }

        if (population < 0)
        {
            throw new UserFriendlyException("Population cannot be less than zero.");
        }

        return new Region(
            GuidGenerator.Create(),
            subcontinentId,
            name,
            population,
            remarks
        );
    }
}
