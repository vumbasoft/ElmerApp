using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using VumbaSoft.ErmanApp.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace VumbaSoft.ErmanApp.Demographics.Regions;

[Authorize(ErmanAppPermissions.Regions.Default)]
public class RegionAppService : ApplicationService, IRegionAppService
{
    private readonly IRegionRepository _regionRepository;
    private readonly ISubcontinentRepository _subcontinentRepository;
    private readonly RegionManager _regionManager;

    public RegionAppService(
        IRegionRepository regionRepository,
        ISubcontinentRepository subcontinentRepository,
        RegionManager regionManager)
    {
        _regionRepository = regionRepository;
        _subcontinentRepository = subcontinentRepository;
        _regionManager = regionManager;
    }

    public async Task<PagedResultDto<RegionDto>> GetListAsync(GetRegionsInput input)
    {
        var totalCount = await _regionRepository.GetCountAsync();
        var regions = await _regionRepository.GetListAsync(
            input.FilterText,
            input.SubcontinentId,
            input.MaxResultCount,
            input.SkipCount,
            input.Sorting
        );

        var regionDtos = ObjectMapper.Map<List<Region>, List<RegionDto>>(regions);
        await SetSubcontinentNamesAsync(regionDtos);

        return new PagedResultDto<RegionDto>(totalCount, regionDtos);
    }

    public async Task<RegionDto> GetAsync(Guid id)
    {
        var region = await _regionRepository.GetAsync(id);
        var regionDto = ObjectMapper.Map<Region, RegionDto>(region);
        await SetSubcontinentNamesAsync(new List<RegionDto> { regionDto });
        return regionDto;
    }

    [Authorize(ErmanAppPermissions.Regions.Create)]
    public async Task<RegionDto> CreateAsync(CreateUpdateRegionDto input)
    {
        var region = await _regionManager.CreateAsync(
            input.SubcontinentId, input.Name, input.Population, input.Remarks);

        await _regionRepository.InsertAsync(region);
        return ObjectMapper.Map<Region, RegionDto>(region);
    }

    [Authorize(ErmanAppPermissions.Regions.Edit)]
    public async Task<RegionDto> UpdateAsync(Guid id, CreateUpdateRegionDto input)
    {
        var region = await _regionRepository.GetAsync(id);

        region.SetSubcontinentId(input.SubcontinentId);
        region.SetName(input.Name);
        region.SetPopulation(input.Population);
        region.SetRemarks(input.Remarks);

        await _regionRepository.UpdateAsync(region);
        return ObjectMapper.Map<Region, RegionDto>(region);
    }

    [Authorize(ErmanAppPermissions.Regions.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _regionRepository.DeleteAsync(id);
    }

    private async Task SetSubcontinentNamesAsync(IReadOnlyCollection<RegionDto> regions)
    {
        if (regions.Count == 0)
        {
            return;
        }

        var subcontinentIds = regions.Select(r => r.SubcontinentId).Distinct().ToList();
        var subcontinents = await _subcontinentRepository.GetListAsync(s => subcontinentIds.Contains(s.Id));
        var subcontinentLookup = subcontinents.ToDictionary(s => s.Id, s => s.Name);

        foreach (var region in regions)
        {
            region.SubcontinentName = subcontinentLookup.GetValueOrDefault(region.SubcontinentId) ?? string.Empty;
        }
    }
}
