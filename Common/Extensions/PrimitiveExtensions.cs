namespace Common.Extensions;
public static class PrimitiveExtensions
{
    public static bool In<T>(this T primitive, params T[] @params)
    {
        return @params.Contains(primitive);
    }

    public static bool IsPositive(this int primitive)
    {
        return primitive > 0;
    }
}
