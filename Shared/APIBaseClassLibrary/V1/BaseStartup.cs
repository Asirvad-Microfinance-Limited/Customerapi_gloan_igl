//using APIBaseClassLibrary.V1.ExceptionHandler;
using APIBaseClassLibrary.V1.ExceptionHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIBaseClassLibrary.V1
{
    public class BaseStartup
    {
        public void baseConfigureServices(IServiceCollection services, string apiName, string apiVersion)
        {
            string[] corsOrigin = { "http://localhost:21314", "http://localhost:4200", "https://mafildev.mactech.net.in", "https://mafiltest.mactech.net.in", "https://goldloan.manappuram.net", "https://goldservices.manappuram.net", "https://mac.mactech.net.in" };

            services.AddSwaggerGen(options =>
            {

                // swagger added for documentation
                options.SwaggerDoc(apiVersion, new Info { Title = apiName, Version = apiVersion });


                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });

                options.AddSecurityDefinition("Bearer1", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization1",
                    In = "header",
                    Type = "apiKey"
                });

                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer1", new string[] { } }
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("fiver",
                    policy => policy.WithOrigins(corsOrigin).AllowAnyHeader()
               .AllowAnyMethod().AllowCredentials());
            });
            services.AddMvc(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("fiver"));

            });
           
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = 10000000;
                x.MultipartBodyLengthLimit = 10000000;
                x.MultipartHeadersLengthLimit = 10000000;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void baseConfigure(IApplicationBuilder app, IHostingEnvironment env, string apiName)
        {

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("../swagger/v1/swagger.json", apiName);
            });

            app.UseMvc(routes =>
            {
                // SwaggerGen won't find controllers that are routed via this technique.
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }
}
