using System.IO;
using System.Xml.Serialization;

namespace AugmentationFramework;

/// <summary>
///     Provides extensions for generic types.
/// </summary>
internal static class GenericExtensions
{
    /// <summary>
    ///     Replaces the value of a struct with a new non-null value.
    /// </summary>
    /// <typeparam name="T">The type of the struct.</typeparam>
    /// <param name="obj">The object to potentially assign <paramref name="val" /> to.</param>
    /// <param name="val">The value that replaces <paramref name="obj" /> if <paramref name="val" /> is not null.</param>
    /// <returns>
    ///     <paramref name="obj" /> if <paramref name="val" /> is null, <paramref name="obj" /> with the value of
    ///     <paramref name="val" /> otherwise.
    /// </returns>
    public static T IfNotNull<T>(this T obj, T? val) where T : struct
    {
        if (val.HasValue)
        {
            obj = val.Value;
        }

        return obj;
    }

    /// <summary>
    ///     Replaces the value of a class with a new non-null value.
    /// </summary>
    /// <typeparam name="T">The type of the class.</typeparam>
    /// <param name="obj">The object to potentially assign <paramref name="val" /> to.</param>
    /// <param name="val">The value that replaces <paramref name="obj" /> if <paramref name="val" /> is not null.</param>
    /// <returns>
    ///     <paramref name="obj" /> if <paramref name="val" /> is null, <paramref name="obj" /> with the value of
    ///     <paramref name="val" /> otherwise.
    /// </returns>
    public static T IfNotNull<T>(this T obj, T? val) where T : class
    {
        if (val != null)
        {
            obj = val;
        }

        return obj;
    }

    /// <summary>
    ///     Clones an object via XML serialization.
    /// </summary>
    /// <typeparam name="T">The type of the object to clone.</typeparam>
    /// <param name="obj">The object to clone.</param>
    /// <returns> The cloned object.</returns>
    public static T Clone<T>(this T obj) where T : class
    {
        using var ms = new MemoryStream();
        var serializer = new XmlSerializer(obj.GetType());
        serializer.Serialize(ms, obj);
        ms.Seek(0, SeekOrigin.Begin);
        return (T)serializer.Deserialize(ms)!;
    }
}