using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NorthTraderAPI.SwaggerConfigurations
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public void Configure(SwaggerGenOptions options)
        {
           foreach(var desc in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(desc.GroupName, CreateInfoForApiVersion(desc));
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription desc)
        {
            var info = new OpenApiInfo()
            {
                Title = "NorthwindTraderApi",
                Version = desc.ApiVersion.ToString(),
                Description= "Northwind Trading Co.",
                Contact = new OpenApiContact()
                {
                    Name = "Urgen Dorjee",
                    Email = "urgen0240@gmail.com"
                },
                License = new OpenApiLicense()
                {
                    Name = "Northwind Trading Registered License"
                }                
            };

            return info;
        }

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
          _provider = provider;
        }
    }
}
