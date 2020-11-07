using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BooksLibrary.API.Data.Repositories;
using BooksLibrary.API.Data.StorageProviders;
using BooksLibrary.API.Data.Database;
using BooksLibrary.API.Entities;
using BooksLibrary.API.Data.Database.Queries;
using BooksLibrary.API.Data.StorageProviders.SQLiteProvider;

namespace BooksLibrary.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IStorageProvider<Book>, BookStorageProvider>();
            services.AddScoped<IStorageProvider<Author>, AuthorStorageProvider>();
            services.AddScoped<IStorageProvider<Category>, CategoryStorageProvider>();
            services.AddSingleton<IDatabaseConfiguration, DatabaseConfiguration>();
            services.AddSingleton<IQueryReader, QueryReader>();
            services.AddSingleton<IQueryCommand, QueryCommand>();
            services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();
            services.AddScoped<IDataRepository<Book>, BookRepository>();
            services.AddScoped<IDataRepository<Author>, AuthorRepository>();
            services.AddScoped<IDataRepository<Category>, CategoryRepository>();

            services.AddCors(o => o.AddPolicy("AllowCors", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            }));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowCors");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            serviceProvider.GetService<IDatabaseBootstrap>().Setup(false);
        }
    }
}
