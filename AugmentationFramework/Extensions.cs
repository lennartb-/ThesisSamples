using System.IO;
using System.Xml.Serialization;

namespace AugmentationFramework;

public static class Extensions
{
    public static T Clone<T>(this T obj) where T: class
    {
        using var ms = new MemoryStream();
        XmlSerializer serializer = new XmlSerializer(obj.GetType());
        serializer.Serialize(ms, obj);
        ms.Seek(0, SeekOrigin.Begin);
        return (T)serializer.Deserialize(ms)!;
    }
}
