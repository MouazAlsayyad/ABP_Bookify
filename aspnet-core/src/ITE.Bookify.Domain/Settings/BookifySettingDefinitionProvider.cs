using Volo.Abp.Settings;

namespace ITE.Bookify.Settings;

public class BookifySettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(BookifySettings.MySetting1));
    }
}
