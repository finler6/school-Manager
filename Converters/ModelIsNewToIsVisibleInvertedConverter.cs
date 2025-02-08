using CommunityToolkit.Maui.Converters;
using SchoolManage.BL.Models;
using System.Globalization;

namespace SchoolManage.App.Converters;

public class ModelIsNewToIsVisibleInvertedConverter : BaseConverterOneWay<ModelBase, bool>
{
    public override bool ConvertFrom(ModelBase value, CultureInfo? culture)
        => value.Id != Guid.Empty;

    public override bool DefaultConvertReturnValue { get; set; } = true;
}
