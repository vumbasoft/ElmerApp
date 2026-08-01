using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace VumbaSoft.ErmanApp.Demographics.Continents;

public class ContinentManager : DomainService
{
    private readonly IContinentRepository _continentRepository;

    public ContinentManager(IContinentRepository continentRepository)
    {
        _continentRepository = continentRepository;
    }

    public async Task<Continent> CreateAsync(string name, long population = 0, string? remarks = null)
    {
        if (await _continentRepository.FindByNameAsync(name) != null)
        {
            throw new UserFriendlyException($"A continent with name '{name}' already exists!");
        }

        return new Continent(
            GuidGenerator.Create(),
            name,
            population,
            remarks
        );
    }
}
