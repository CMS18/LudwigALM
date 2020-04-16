using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudwigALM.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LudwigALM
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc(options => options.EnableEndpointRouting = false);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            BankRepository.AddCustomers(CreateCustomers());
        }

        private List<Customer> CreateCustomers()
        {
            return new List<Customer>()
            {
                CreateCustomer(1, "Kasper",   new decimal[] {50M, 10000M}, new int[] {  1,  2}),
                CreateCustomer(2, "Bert",    new decimal[] {14M, 20M}, new int[] {  3,  4}),
                CreateCustomer(3, "Matts", new decimal[] {70M, 300M}, new int[] {  5,  6}),
                CreateCustomer(4, "David",   new decimal[] {81M, 40000M}, new int[] {  7,  8}),
                CreateCustomer(5, "Erik",    new decimal[] {90000M, 50000M}, new int[] {  9, 10}),
                CreateCustomer(6, "Maja", new decimal[] {40000M, 60000M}, new int[] { 11, 12})
            };
        }

        private Customer CreateCustomer(int id, string name, decimal[] balances, int[] accountIds)
        {
            var accounts = new List<Account>();
            for (int k=0; k<balances.Count(); k++)
            {
                accounts.Add(new Account() { AccountId = accountIds[k], Balance = balances[k] });
            }
            return new Customer() {
                Id = id,
                Name = name,
                Accounts = accounts
            };
        }
    }
}
