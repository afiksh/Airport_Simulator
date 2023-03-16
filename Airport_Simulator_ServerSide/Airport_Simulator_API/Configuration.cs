using Models.Models;
using DataBase;
using DataBase.Repository;
using Microsoft.AspNetCore.SignalR.Client;
#region using
using Microsoft.EntityFrameworkCore;
#endregion

namespace Airport_Simulator_API
{
    /// <summary>
    /// Configure the <seealso cref="WebApplication"/>
    /// </summary>
    public class Configuration
    {
        #region Fields
        WebApplicationBuilder builder;
        #endregion

        #region Consturctors
        public Configuration(string[] args)
        {
            builder = WebApplication.CreateBuilder(args);
            OnConfigure();
        }
        #endregion

        #region Methohds

        /// <summary>
        /// Add services to <see cref="builder"/>
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        private void OnConfiguration()
        {
            #region Services

            #region Singeltons
   
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Airport")
                ?? throw new InvalidOperationException("Connection string 'Airport' not found.")), ServiceLifetime.Singleton);

            builder.Services.AddSingleton<Queue<Airplane>>();

            builder.Services.AddSingleton<IRepository<Airplane>, Repository>();
            
            #region HubConnection

            HubConnection hubConnection = new HubConnectionBuilder()
                                            .WithUrl("https://localhost:7247/ConnectionHub")
                                            .WithAutomaticReconnect()
                                            .Build();
            builder.Services.AddSingleton(hubConnection);

            #endregion

            #endregion

            builder.Services.AddSignalR();

            builder.Services.AddMemoryCache();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            //Cross-Origin Resource Sharing
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "cors",
                    policy =>
                    {
                        policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });
            #endregion
        }

        /// <summary>
        /// Middlewares
        /// </summary>
        private void OnConfigure()
        {
            OnConfiguration();

            var app = builder.Build();

            app.UseCors("cors");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<ConnectionHub>(new PathString("/ConnectionHub"));

            app.Run();
        }

        #endregion
    }
}
