using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLibrary;
using GlobalValues;
using KYCAPI.V1.BLL;
using KYCAPI.V1.BLLDependency;
using KYCAPI.V1.Models.Appsettings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using static GlobalValues.GlobalVariables;

namespace KYCAPI
{
    public class Startup : APIBaseClassLibrary.V1.BaseStartup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            baseConfigureServices(services, "KYC API", "v1");

            services.AddTransient<ICKYCBLL, CKYCBLL>();
            services.AddTransient<IConsentBLL, ConsentBLL>();
            services.AddTransient<IEKYCBLL, EKYCBLL>();
            services.AddTransient<IJurisdictionBLL, JurisdictionBLL>();
            services.AddTransient<IKYCBLL, KYCBLL>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            baseConfigure(app, env, "KYC API");
        }
    }
}
