using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NorthTraderAPI.SwaggerConfigurations;

public class SwaggerDefaultValues : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
		foreach (var para in operation.Parameters)
		{
			var desc = context.ApiDescription.ParameterDescriptions
				.First(p => p.Name == para.Name);

			para.Description ??= desc.ModelMetadata?.Description;

			if (para.Schema.Default == null && desc.DefaultValue != null)
			{
				para.Schema.Default = new OpenApiString(desc.DefaultValue.ToString());
			}

			para.Required |= desc.IsRequired;
		}
	}
}
