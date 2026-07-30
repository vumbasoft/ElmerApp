using VumbaSoft.ErmanApp.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace VumbaSoft.ErmanApp.Permissions;

public class ErmanAppPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ErmanAppPermissions.GroupName);

        var booksPermission = myGroup.AddPermission(ErmanAppPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(ErmanAppPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(ErmanAppPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(ErmanAppPermissions.Books.Delete, L("Permission:Books.Delete"));

        var authorsPermission = myGroup.AddPermission(ErmanAppPermissions.Authors.Default, L("Permission:Authors"));
        authorsPermission.AddChild(ErmanAppPermissions.Authors.Create, L("Permission:Authors.Create"));
        authorsPermission.AddChild(ErmanAppPermissions.Authors.Edit, L("Permission:Authors.Edit"));
        authorsPermission.AddChild(ErmanAppPermissions.Authors.Delete, L("Permission:Authors.Delete"));

        var continentsPermission = myGroup.AddPermission(ErmanAppPermissions.Continents.Default, L("Permission:Continents"));
        continentsPermission.AddChild(ErmanAppPermissions.Continents.Create, L("Permission:Continents.Create"));
        continentsPermission.AddChild(ErmanAppPermissions.Continents.Edit, L("Permission:Continents.Edit"));
        continentsPermission.AddChild(ErmanAppPermissions.Continents.Delete, L("Permission:Continents.Delete"));

        var subcontinentsPermission = myGroup.AddPermission(ErmanAppPermissions.Subcontinents.Default, L("Permission:Subcontinents"));
        subcontinentsPermission.AddChild(ErmanAppPermissions.Subcontinents.Create, L("Permission:Subcontinents.Create"));
        subcontinentsPermission.AddChild(ErmanAppPermissions.Subcontinents.Edit, L("Permission:Subcontinents.Edit"));
        subcontinentsPermission.AddChild(ErmanAppPermissions.Subcontinents.Delete, L("Permission:Subcontinents.Delete"));

        var regionsPermission = myGroup.AddPermission(ErmanAppPermissions.Regions.Default, L("Permission:Regions"));
        regionsPermission.AddChild(ErmanAppPermissions.Regions.Create, L("Permission:Regions.Create"));
        regionsPermission.AddChild(ErmanAppPermissions.Regions.Edit, L("Permission:Regions.Edit"));
        regionsPermission.AddChild(ErmanAppPermissions.Regions.Delete, L("Permission:Regions.Delete"));

        var countriesPermission = myGroup.AddPermission(ErmanAppPermissions.Countries.Default, L("Permission:Countries"));
        countriesPermission.AddChild(ErmanAppPermissions.Countries.Create, L("Permission:Countries.Create"));
        countriesPermission.AddChild(ErmanAppPermissions.Countries.Edit, L("Permission:Countries.Edit"));
        countriesPermission.AddChild(ErmanAppPermissions.Countries.Delete, L("Permission:Countries.Delete"));

        var stateProvincesPermission = myGroup.AddPermission(ErmanAppPermissions.StateProvinces.Default, L("Permission:StateProvinces"));
        stateProvincesPermission.AddChild(ErmanAppPermissions.StateProvinces.Create, L("Permission:StateProvinces.Create"));
        stateProvincesPermission.AddChild(ErmanAppPermissions.StateProvinces.Edit, L("Permission:StateProvinces.Edit"));
        stateProvincesPermission.AddChild(ErmanAppPermissions.StateProvinces.Delete, L("Permission:StateProvinces.Delete"));

        var districtCitiesPermission = myGroup.AddPermission(ErmanAppPermissions.DistrictCities.Default, L("Permission:DistrictCities"));
        districtCitiesPermission.AddChild(ErmanAppPermissions.DistrictCities.Create, L("Permission:DistrictCities.Create"));
        districtCitiesPermission.AddChild(ErmanAppPermissions.DistrictCities.Edit, L("Permission:DistrictCities.Edit"));
        districtCitiesPermission.AddChild(ErmanAppPermissions.DistrictCities.Delete, L("Permission:DistrictCities.Delete"));

        var localitiesPermission = myGroup.AddPermission(ErmanAppPermissions.Localities.Default, L("Permission:Localities"));
        localitiesPermission.AddChild(ErmanAppPermissions.Localities.Create, L("Permission:Localities.Create"));
        localitiesPermission.AddChild(ErmanAppPermissions.Localities.Edit, L("Permission:Localities.Edit"));
        localitiesPermission.AddChild(ErmanAppPermissions.Localities.Delete, L("Permission:Localities.Delete"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(ErmanAppPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ErmanAppResource>(name);
    }
}
