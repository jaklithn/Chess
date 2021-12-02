namespace Chess.Extensions;

public static class EnumExtensions
{
    public static T ToEnum<T>(this string value) where T : Enum
    {
        if (!Enum.IsDefined(typeof(T), value))
        {
            throw new ApplicationException($"String value='{value}' can not be interpreted as valid enum of type {typeof(T).Name}");
        }

        return (T)Enum.Parse(typeof(T), value);
    }

    public static T ToEnum<T>(this int value) where T : Enum
    {
        if (!Enum.IsDefined(typeof(T), value))
        {
            throw new ApplicationException($"Integer value={value} can not be interpreted as valid enum of type {typeof(T).Name}");
        }

        return (T)Enum.ToObject(typeof(T), value);
    }

    public static List<T> ToList<T>()
    {
        EnsureEnum<T>();
        return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    }

    private static void EnsureEnum<T>()
    {
        var enumType = typeof(T);

        // Can't use type constraints on value types, so have to do check like this
        if (enumType.BaseType != typeof(Enum))
        {
            throw new ArgumentException("T must be of type System.Enum");
        }
    }
}