using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

public static class EnumHelper
{
    public static string GetDisplayValue(Enum value)
    {
        Type type = value.GetType();
        string name = Enum.GetName(type, value);
        if (name == null)
        {
            return null;
        }

        FieldInfo field = type.GetField(name);
        if (field == null)
        {
            return null;
        }

        DisplayAttribute attr = field.GetCustomAttribute<DisplayAttribute>();
        if (attr == null)
        {
            return name;
        }

        return attr.Name;
    }
}
