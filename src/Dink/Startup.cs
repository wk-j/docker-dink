using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dink.Controllers;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dink {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
            services.AddSingleton<IConverter>(new SynchronizedConverter(new PdfTools { }));
            services.AddHttpClient();
            services.AddSingleton<GlobalService>();
            services.AddMvc();
        }

        int FindPort(IApplicationBuilder app) {
            var address = app.ServerFeatures
               .Get<IServerAddressesFeature>().Addresses.First();
            var port = int.Parse(address.Split(':').Last());

            if (port == 5001) {
                return 5000;
            }

            return port;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, GlobalService global) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            var port = FindPort(app);
            global.ServerPort = port;

            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
