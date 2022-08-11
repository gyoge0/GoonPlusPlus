using Avalonia.Media;
using GoonPlusPlus.ViewModels;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;

namespace GoonPlusPlus.Util.Serialization;

/// <summary>
///     Custom attribute for the default JSON string representation of a property on <see cref="CustomCfg" />
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class CustomCfgDefaultAttribute : Attribute
{
    // WARNING
    // Adding extra arguments requires changing the LINQ query in CustomCfgSerializer.WriteJson
    public CustomCfgDefaultAttribute(string name) => Name = name;

    // This is accessed by reflection
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string Name { get; }
}

public class CustomCfgSerializer : JsonConverter<CustomCfg>
{
    public override bool CanRead => false;

    public override void WriteJson(JsonWriter writer, CustomCfg? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteStartObject();
            writer.WriteEndObject();
            writer.Close();
            return;
        }

        writer.WriteStartObject();
        value.GetType()
            .GetProperties()
            .Where(p =>
            {
                var attrs = p.CustomAttributes.Select(a => a.AttributeType).ToArray();
                return !attrs.Contains(typeof(JsonIgnoreAttribute))
                    && attrs.Contains(typeof(CustomCfgDefaultAttribute));
            })
            .Where(p => p.CustomAttributes.Single(a => a.AttributeType == typeof(CustomCfgDefaultAttribute))
                .ConstructorArguments.Single()
                .Value as string != ValueToString(p.GetValue(value)))
            .ToList()
            .ForEach(p =>
            {
                writer.WritePropertyName(ToSnakeCase(p.Name));
                writer.WriteRawValue(ValueToString(p.GetValue(value)));
            });
        writer.WriteEndObject();
        writer.Close();
    }

    private static string ValueToString(object? value) => value switch
    {
        FontFamily f => '"' + string.Join(",", f.FamilyNames) + '"',
        double d => $"{d}",
        bool b => b ? "true" : "false",
        IBrush or FontStyle or FontWeight => '"' + value.ToString() + '"',
        _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
    };

    /// <summary>
    ///     Converts UpperCamelCaseText to lower_snake_case_text
    /// </summary>
    /// <param name="text">Text to convert</param>
    /// <returns>Text in lower_snake_case</returns>
    /// <exception cref="ArgumentNullException">If Text is null</exception>
    private static string ToSnakeCase(string text)
    {
        if (text == null) throw new ArgumentNullException(nameof(text));
        if (text.Length < 2) return text;

        var sb = new StringBuilder();
        sb.Append(char.ToLowerInvariant(text[0]));
        for (var i = 1; i < text.Length; ++i)
        {
            var c = text[i];
            if (char.IsUpper(c))
            {
                sb.Append('_');
                sb.Append(char.ToLowerInvariant(c));
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }

    public override CustomCfg ReadJson(
        JsonReader reader, Type objectType, CustomCfg? existingValue, bool hasExistingValue, JsonSerializer serializer
    ) => throw new NotImplementedException();
}
