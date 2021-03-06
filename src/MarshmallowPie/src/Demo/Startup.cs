using HotChocolate;
using HotChocolate.AspNetCore;
using MarshmallowPie.GraphQL;
using MarshmallowPie.Repositories.Mongo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using MarshmallowPie;

namespace Demo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMongoRepositories(sp => new MongoClient().GetDatabase("foo3"));

            services.AddPublishDocumentService();

            services.AddInMemoryMessageQueue();
            services.AddFileSystemStorage("temp");

            services.AddSchemaRegistryDataLoader();

            services.AddGraphQL(
                SchemaBuilder.New()
                    .AddSchemaRegistry()
                    .EnableRelaySupport());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseWebSockets();

            app.UseGraphQL();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!").ConfigureAwait(false);
                });
            });
        }
    }
}
