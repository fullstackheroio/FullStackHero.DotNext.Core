namespace FullStackHero.DotNext.Core.Extensions;

public static class XmlExtension
{
    /// <summary>
    ///     Deserialize the string to object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static T? DeserializeXml<T>(this string xml) where T : class
    {
        if (string.IsNullOrWhiteSpace(xml)) throw new ArgumentNullException(nameof(xml), "Chuỗi XML không được null hoặc empty.");

        using var stringReader = new StringReader(xml);
        var       serializer   = new XmlSerializer(typeof(T));

        return serializer.Deserialize(stringReader) as T;
    }

    /// <summary>
    ///     Serialize object to string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="useNamespaces"></param>
    /// <param name="indent"></param>
    /// <returns></returns>
    public static string SerializeXml<T>(this T obj, bool useNamespaces = false, bool indent = false) where T : class
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj), "Đối tượng cần serialize không được null.");

        var       serializer   = new XmlSerializer(typeof(T));
        using var stringWriter = new StringWriter();

        var settings = new XmlWriterSettings
        {
            Async              = true,
            Indent             = indent,
            Encoding           = Encoding.UTF8,
            OmitXmlDeclaration = !useNamespaces
        };

        using var xmlWriter = XmlWriter.Create(stringWriter, settings);

        switch (useNamespaces)
        {
            case false:
                var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });
                serializer.Serialize(xmlWriter, obj, namespaces);

                break;

            case true:
                serializer.Serialize(xmlWriter, obj);

                break;
        }

        return stringWriter.ToString();
    }
}