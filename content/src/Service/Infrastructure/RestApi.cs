using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace MyVendor.MyService.Infrastructure
{
    public static class RestApi
    {
        public static IServiceCollection AddRestApi(this IServiceCollection services)
        {
            services.AddMvc(options => options.Filters.Add(typeof(ApiExceptionFilterAttribute)))
                    .AddJsonOptions(options =>
                     {
                         options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                         options.SerializerSettings.Converters.Add(new StringEnumConverter {CamelCaseText = true});
                     });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "My Service",
                        Version = "v1",
                        Contact = new Contact
                        {
                            Email = "developer@example.com",
                            Url = "http://www.example.com"
                        }
                    });
                options.IncludeXmlComments(Path.Combine(ApplicationEnvironment.ApplicationBasePath, "MyVendor.MyService.xml"));
                options.IncludeXmlComments(Path.Combine(ApplicationEnvironment.ApplicationBasePath, "MyVendor.MyService.Dto.xml"));
                options.DescribeAllEnumsAsStrings();
            });

            return services;
        }

        public static IApplicationBuilder UseRestApi(this IApplicationBuilder app)
        {
            if (app.ApplicationServices.GetRequiredService<IHostingEnvironment>().IsDevelopment())
            {
                app.UseDeveloperExceptionPage()
                   .UseSwagger()
                   .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Service API v1"));
            }

            return app.UseMvc();
        }
    }
}
