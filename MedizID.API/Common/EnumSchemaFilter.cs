using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace MedizID.API.Common;

/// <summary>
/// SchemaFilter to add x-ms-enum extension for Kiota code generation.
/// This allows Kiota to generate proper C# enum types instead of int values.
/// </summary>
public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type != null && context.Type.IsEnum)
        {
            var enumType = context.Type;
            var enumValues = Enum.GetNames(enumType);
            var enumValueDescriptions = new Dictionary<string, int>();

            foreach (var enumValue in enumValues)
            {
                var enumMember = enumType.GetMember(enumValue).First();
                var enumIntValue = (int)Enum.Parse(enumType, enumValue);
                enumValueDescriptions[enumValue] = enumIntValue;
            }

            // Add x-ms-enum extension for Kiota
            var enumArray = new OpenApiArray();
            foreach (var name in enumValues)
            {
                var enumObj = new OpenApiObject
                {
                    { "name", new OpenApiString(name) },
                    { "value", new OpenApiInteger(enumValueDescriptions[name]) }
                };
                enumArray.Add(enumObj);
            }

            schema.Extensions["x-ms-enum"] = new OpenApiObject
            {
                { "name", new OpenApiString(enumType.Name) },
                { "modelAsString", new OpenApiBoolean(false) },
                { "values", enumArray }
            };
        }
    }
}
