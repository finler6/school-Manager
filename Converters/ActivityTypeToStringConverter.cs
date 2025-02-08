using CommunityToolkit.Maui.Converters;
using SchoolManage.App.Resources.Texts;
using SchoolManage.Common.Enums;
using System.Globalization;

namespace SchoolManage.App.Converters;

public class ActivityTypeToStringConverter : BaseConverterOneWay<ActivityType, string>
{
    public override string ConvertFrom(ActivityType value, CultureInfo? culture)
        => ActivityTypeTexts.ResourceManager.GetString(value.ToString(), culture)
           ?? ActivityTypeTexts.None;
    public override string DefaultConvertReturnValue { get; set; } = ActivityTypeTexts.None;
}
