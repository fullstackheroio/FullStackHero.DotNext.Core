namespace FullStackHero.DotNext.Core.Extensions;

public static class AttributeExtension
{
    /// <summary>
    ///     Hàm đọc giá trị được khai báo trong thuộc tính <see cref="DescriptionAttribute" />
    ///     của đối tượng enum, field.
    /// </summary>
    /// <typeparam name="TField"></typeparam>
    /// <param name="field"></param>
    /// <returns></returns>
    public static string? ToDescriptionString<TField>(this TField field) =>
        typeof(TField).GetField(field?.ToString()!)
                      ?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                      .Cast<DescriptionAttribute>()
                      .FirstOrDefault()
                      ?.Description ?? field?.ToString();

    /// <summary>
    ///     Lấy Attribute được gán cho Property.
    /// </summary>
    /// <typeparam name="T">Lớp có chứa property với tên <see cref="propertyName" /></typeparam>
    /// <typeparam name="TAttribute">Thuộc tính muốn trích xuất</typeparam>
    /// <param name="propertyName">Tên của property</param>
    /// <returns></returns>
    public static TAttribute? GetAttribute<T, TAttribute>(this string propertyName) where T : class
                                                                                    where TAttribute : Attribute =>
        string.IsNullOrWhiteSpace(propertyName) ? default : typeof(T).GetProperty(propertyName)?.GetCustomAttribute<TAttribute>();

    /// <summary>
    ///     Lấy attribute của class.
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public static TAttribute? GetAttribute<TAttribute>(this Type type) where TAttribute : Attribute => type.GetCustomAttribute<TAttribute>();
}