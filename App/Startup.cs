using System.Data;
using System.Data.SqlClient;
using App.Data;
using App.Hub;
using App.Messaging;
using App.Models;
using App.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var customSettings = CustomSettingsConfiguration(services, Configuration);

            services.AddSingleton(services => new DbConnectionFactory<SqlConnection>(customSettings.ConnectionString));
            services.AddTransient<IDbConnection>(services =>
                        services.GetService<DbConnectionFactory<SqlConnection>>().GetConnection());

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.RegisterMessaging();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserDeviceService, UserDeviceService>();

            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials()
                           .WithOrigins("http://localhost:3000", "http://localhost:49156", "https://localhost:49156")

                        );
            });
            services.AddSignalR(e => {
                e.MaximumReceiveMessageSize = 102400000;
            });
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHub<NotificationHub>("/hubs");
                endpoints.MapHub<NotificationHubBasic>("/hubs");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V2");
            });
        }

        private static ICustomSettings CustomSettingsConfiguration(IServiceCollection services, IConfiguration config)
        {
            var customSettingsSection = config.GetSection("CustomSettings");
            var customSettings = customSettingsSection.Get<CustomSettings>();

            return customSettings;
        }
    }
}

