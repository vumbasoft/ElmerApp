using System;
using System.Threading.Tasks;
using VumbaSoft.ErmanApp.Demographics.Continents;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace VumbaSoft.ErmanApp.Demographics.Subcontinents;

public class SubcontinentManager : DomainService
{
    private readonly ISubcontinentRepository _subcontinentRepository;
    private readonly IContinentRepository _continentRepository;

    public SubcontinentManager(
        ISubcontinentRepository subcontinentRepository,
        IContinentRepository continentRepository)
    {
        _subcontinentRepository = subcontinentRepository;
        _continentRepository = continentRepository;
    }

    public async Task<Subcontinent> CreateAsync(
        Guid continentId, string name, long population = 0, string? remarks = null)
    {
        var continent = await _continentRepository.FindAsync(continentId);
        if (continent == null)
        {
            throw new UserFriendlyException($"Continent with id '{continentId}' was not found!");
        }

        if (await _subcontinentRepository.FindByNameAsync(name) != null)
        {
            throw new UserFriendlyException($"A subcontinent with name '{name}' already exists!");
        }

        if (population < 0)
        {
            throw new UserFriendlyException("Population cannot be less than zero.");
        }

        return new Subcontinent(
            GuidGenerator.Create(),
            continentId,
            name,
            population,
            remarks
        );
    }
}
