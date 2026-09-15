using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MauiPersianToolkit.Extensions;

public static class EnumExtensions
{
    public static string GetDisplay(this Type type, int value)
    {
        var enumItem = Enum.Parse(type, value.ToString());
        var displayName = type.GetField(enumItem.ToString()).GetCustomAttribute<DisplayAttribute>().Name;
        return displayName;
    }

    public static T GetValueByDisplay<T>(string displayName) where T : Enum
    {
        var type = typeof(T);
        foreach (var value in type.GetEnumValues())
        {
            var field = Enum.Parse(type, value.ToString());
            var displayAttribute = type.GetField(field.ToString()).GetCustomAttribute<DisplayAttribute>();

            if (displayAttribute != null && displayAttribute.Name == displayName)
            {
                return (T)field;
            }
        }

        return default(T);
    }
}
