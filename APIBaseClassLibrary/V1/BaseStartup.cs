using APIBaseClassLibrary.V1.Controllers;
using APIBaseClassLibrary.V1.ExceptionHandler;
using APIBaseClassLibrary.V1.Log;
using APIBaseClassLibrary.V1.TokenAttribute;
using DataAccessLibrary;
using GlobalValues;
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
            // string[] corsOrigin = { "http://localhost:21314", "http://localhost:4200", "https://mafildev.mactech.net.in", "https://mac.mactech.net.in", "https://qc.mactech.net.in", "https://mafiltest.mactech.net.in", "https://goldloan.manappuram.net", "https://goldservices.manappuram.net", "https://mahofin.macomsolutions.in", "https://demo.mactech.net.in" };
            string[] corsOrigin = { "http://localhost:21314", "http://localhost:4200", "https://mafildev.mactech.net.in","https://goldloan.manappuram.net", "https://goldservices.manappuram.net"};
            // string[] methods = { "GET", "POST", "PUT", "DELETE"};

            //services.AddSwaggerGen(options =>
            //{

            //    // swagger added for documentation
            //    options.SwaggerDoc(apiVersion, new Info { Title = apiName, Version = apiVersion });



            //    options.AddSecurityDefinition("Bearer", new ApiKeyScheme
            //    {
            //        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            //        Name = "Authorization",
            //        In = "header",
            //        Type = "apiKey"
            //    });
            //    options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
            //    {
            //        { "Bearer", new string[] { } }
            //    });

            //});

            services.AddCors(options =>
            {
                options.AddPolicy("fiver",
                    policy => policy.WithOrigins(corsOrigin).AllowAnyHeader()
               .AllowAnyMethod().AllowCredentials());
            });
            services.AddMvc(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("fiver"));
              //  options.Filters.Add(new  ExceptionMiddlewareExtensions());

            });
            //
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.AddTransient<IToken, Token>();
            services.AddTransient<TokenValidator>();
            services.AddTransient<ExceptionMiddlewareExtensions>();
            
            services.AddTransient<LogAttribute>();
            services.AddTransient<IDBAccessHelper, DBAccessHelper>();
            services.AddTransient<IGlobalMethods, GlobalMethods>();
            services.AddTransient<IDBAccessDapperHelper, DBAccessDapperHelper>();
            services.AddTransient<IPasswordSecurity, PasswordSecurity>();
            services.AddTransient<ILogBase, LogBase>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void baseConfigure(IApplicationBuilder app, IHostingEnvironment env, string apiName)
        {

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("../swagger/v1/swagger.json", apiName);
            });
            app.UseConventionalMiddleware();
            //app.UseMvc(routes =>
            //{
            //    // SwaggerGen won't find controllers that are routed via this technique.
            //    routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            //});
            //app.UseCors("fiver");
        }

    }
}
