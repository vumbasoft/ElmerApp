using Volo.Abp.Settings;

namespace VumbaSoft.ErmanApp.Settings;

public class ErmanAppSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(ErmanAppSettings.MySetting1));
    }
}
