using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POKEMONLIBRARY.Configuration;
using POKEMONSHOP.Areas.Identity;
using POKEMONSHOP.Data;
using POKEMONSHOP.Contracts;
using POKEMONSHOP.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POKEMONSHOP.MiddleWare;
using Microsoft.AspNetCore.ResponseCompression;
using System.Net.Http;

namespace POKEMONSHOP
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
            // Identity context
            services.AddDbContext<IdentityPokemonDbContext>(options =>
                                                                    options.UseSqlServer(
                                                                                            Configuration.GetConnectionString("IdentityConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<IdentityPokemonDbContext>();

            // Main context
            services.AddDbContext<PokemonDbContext>(options =>
                                                                options.UseSqlServer(
                                                                                        Configuration.GetConnectionString("MainConnection")));

            // –егистраци€ HttpClient 
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
            // Server Side Blazor doesn't register HttpClient by default
            if (!services.Any(x => x.ServiceType == typeof(HttpClient)))
            {
                // Setup HttpClient for server side in a client side compatible fashion
                services.AddScoped<HttpClient>(s =>
                {
                    // Creating the URI helper needs to wait until the JS Runtime is initialized, so defer it. 
                    var uriHelper = s.GetRequiredService<NavigationManager>();
                    return new HttpClient
                    {
                        BaseAddress = new Uri(uriHelper.BaseUri)
                    };
                });
            }

            // ƒобавлен сервис дл€ доступа к контексту запроса
            services.AddHttpContextAccessor();

            // 
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddMvc();

            // Registration services
            services.AddScoped<IPokemonService, PokemonService>();
            services.AddScoped<IOrder, OrderService>();
            services.AddScoped<ICustomer, CustomerService>();
            services.AddScoped<IRepository, Repository>();

            // Social LogIn
            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Facebook:AppSecret"];

                facebookOptions.Events = new OAuthEvents()
                {
                    OnRemoteFailure = loginFailureHandler =>
                    {
                        var authProperties = facebookOptions.StateDataFormat.Unprotect(loginFailureHandler.Request.Query["state"]);
                        loginFailureHandler.Response.Redirect("/Identity/Account/Login");
                        loginFailureHandler.HandleResponse();
                        return Task.FromResult(0);
                    }
                };
            });

            // ƒобавлен E-Mail-сервис дл€ отправки сообщений покупателю об успешной регистрации заказа 
            services.AddScoped<IEmailSender, EmailService>(i =>
               new EmailService(
                   Configuration["EmailSender:Host"],
                   Configuration.GetValue<int>("EmailSender:Port"),
                   Configuration.GetValue<bool>("EmailSender:EnableSSL"),
                   Configuration["EmailSender:UserName"],
                   Configuration["EmailSender:Password"]
               )
           );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            // ƒобавление в куки имени пользовател€
            app.Use(async (context, next) =>
            {
                if (!context.Request.Cookies.ContainsKey("UserName"))
                {
                    context.Response.Cookies.Append("UserName", "ѕользователь пока не авторизован");
                }
                else
                {
                    string username = string.Empty;
                    bool? trygetUserName = context.Request.Cookies?.TryGetValue("UserName", out username);
                    if (trygetUserName != null && trygetUserName != false)
                    {
                        context.Response.Cookies.Append("UserName", username);
                    }
                }
                await next.Invoke();
            });

        }
    }
}
