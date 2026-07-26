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
        //Define your own permissions here. Example:
        //myGroup.AddPermission(ErmanAppPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ErmanAppResource>(name);
    }
}
