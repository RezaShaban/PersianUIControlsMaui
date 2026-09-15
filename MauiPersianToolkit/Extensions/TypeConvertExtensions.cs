namespace MauiPersianToolkit;
public static class TypeConvertExtensions
{
    /// <summary>
    /// convert object to int, if value is null then return 0
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static int ToInt(this object val)
    {
        if (val == null)
            return 0;
        if (val.ToString().Contains("."))
            val = val.ToString().Substring(0, val.ToString().IndexOf("."));
        if (int.TryParse(val.ToString(), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.DefaultThreadCurrentCulture, out int result))
            return result;
        else
            return 0;
    }
    public static bool ToBool(this object val)
    {
        if (val == null || val.ToString() == string.Empty)
            return false;
        if (int.TryParse(val.ToString(), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.DefaultThreadCurrentCulture, out int result))
            return result > 0;
        else if (val.ToString().ToLower() == "true")
            return true;
        else
            return false;
    }
    public static async Task<byte[]> ToArrayAsync(this Stream stream)
    {
        if (stream == null)
            return new byte[0];

        using (var mStream = new MemoryStream())
        {
            await stream.CopyToAsync(mStream);
            return mStream.ToArray();
        }
    }
    public static void MapTo<TSource, TResult>(this TSource source, TResult result, string[] excludeProps = null)
    {
        var properties = typeof(TResult).GetProperties();
        var tSource = typeof(TSource);
        foreach (var p in properties)
        {
            if (excludeProps != null && excludeProps.Length > 0 && excludeProps.Any(x => x == p.Name))
                continue;

            if (!p.CanWrite || !tSource.GetProperties().Any(x => x.Name == p.Name))
                continue;

            p.SetValue(result, tSource.GetProperty(p.Name).GetValue(source));
        }

    }

    public static object GetPropertyValue(this object prop, string propertyName)
    {
        return prop.GetType().GetProperty(propertyName).GetValue(prop);
    }
}
