using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ecom.Entities;
using ecom.Repositories;
using ecom.Services;

namespace ecom
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //MVC
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(o => 
                {
                    if (o.SerializerSettings.ContractResolver != null)
                    {
                        var castedResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;
                        castedResolver.NamingStrategy = null; //Json.NET will take property names as defined
                    }
                });
            
            //add httpcontext accessor to be dependency injected
            services.AddHttpContextAccessor();
            //so context will have session state
            services.AddSession(); 

            //DB Context, and instantiate with connection string
            var connString = Configuration["connectionStrings:WHDBConnectionString"];
            services.AddDbContext<StoreDbContext>(o => o.UseSqlServer(connString));

            //register the repository in the container with Scoped lifetime
            services.AddScoped<IFaqsRepository, FaqsRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IPageContentsRepository, PageContentsRepository>();
            services.AddScoped<IShippingRepository, ShippingRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            //register the services in the container with Scoped lifetime
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<ICheckoutService, CheckoutService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IHomeService, HomeService>();
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();

            //display 404/500 in browser
            app.UseStatusCodePages();

            //add session state
            app.UseSession();

            //culture for prices
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            //app.UseRequestLocalization();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
