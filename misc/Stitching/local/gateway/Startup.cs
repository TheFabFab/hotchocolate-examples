using System;
using Demo.Accounts;
using Demo.Inventory;
using Demo.Products;
using Demo.Reviews;
using HotChocolate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo.Gateway
{
    public class Startup
    {
        public const string Accounts = "accounts";
        public const string Inventory = "inventory";
        public const string Products = "products";
        public const string Reviews = "reviews";

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<UserRepository>()
                .AddGraphQLServer(Accounts)
                .ConfigureSchema(schemaBuilder =>
                {
                    schemaBuilder.AddQueryType<Accounts.Query>();
                    schemaBuilder.AddSubscriptionType<Accounts.Subscription>();
                });

            services
                .AddSingleton<InventoryInfoRepository>()
                .AddGraphQLServer(Inventory)
                .ConfigureSchema(schemaBuilder => schemaBuilder.AddQueryType<Inventory.Query>());

            services
                .AddSingleton<ProductRepository>()
                .AddGraphQLServer(Products)
                .ConfigureSchema(schemaBuilder => schemaBuilder.AddQueryType<Products.Query>());

            services
                .AddSingleton<ReviewRepository>()
                .AddGraphQLServer(Reviews)
                .ConfigureSchema(schemaBuilder => schemaBuilder.AddQueryType<Reviews.Query>());

            services
                .AddGraphQLServer()
                //.AddQueryType(d => d.Name("Query"))
                .AddLocalSchema(Accounts)
                .AddLocalSchema(Inventory)
                .AddLocalSchema(Products)
                .AddLocalSchema(Reviews)
                //.AddTypeExtensionsFromFile("./Stitching.graphql")
                ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapGraphQL(); });
        }
    }
}