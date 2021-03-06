﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gestion.Web.Data;
using Gestion.Web.Helpers;
using Gestion.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Gestion.Web
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
            services.AddIdentity<Usuarios, IdentityRole>(cfg =>
            {
                cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                cfg.SignIn.RequireConfirmedEmail = true;
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequiredLength = 6;
            })                
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<DataContext>();

            services.AddDbContext<DataContext>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddTransient<DataContext>();

            services.Configure<ConexionConfiguracion>(Configuration.GetSection("ConnectionStrings"));

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAuthentication()
            .AddCookie()
            .AddJwtBearer(cfg =>
            {
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = this.Configuration["Tokens:Issuer"],
                    ValidAudience = this.Configuration["Tokens:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            this.Configuration["Tokens:Key"]))
                };
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/NotAuthorized";
                options.AccessDeniedPath = "/Account/NotAuthorized";
            });


            services.AddScoped<ITiposProductosRepository, TiposProductosRepository>();
            services.AddScoped<ITiposDocumentosRepository, TiposDocumentosRepository>();
            services.AddScoped<ICategoriasRepository, CategoriasRepository>();
            services.AddScoped<IColoresRepository, ColoresRepository>();
            services.AddScoped<IEtiquetasRepository, EtiquetasRepository>();
            services.AddScoped<IProductosRepository, ProductosRepository>();
            services.AddScoped<IProductosCategoriasRepository, ProductosCategoriasRepository>();
            services.AddScoped<IProductosEtiquetasRepository, ProductosEtiquetasRepository>();
            services.AddScoped<IProductosImagenesRepository, ProductosImagenesRepository>();
            services.AddScoped<IPresupuestosRepository, PresupuestosRepository>();
            services.AddScoped<IProvinciasRepository, ProvinciasRepository>();
            services.AddScoped<ISucursalesRepository, SucursalesRepository>();
            services.AddScoped<IClientesRepository, ClientesRepository>();
            services.AddScoped<ITiposDocumentosRepository, TiposDocumentosRepository>();
            services.AddScoped<IFactoryConnection, FactoryConnection>();

            services.AddScoped<ITiposProductosRepository, TiposProductosRepository>();
            services.AddScoped<ICuentasVentasRepository, CuentasVentasRepository>();
            services.AddScoped<ICuentasComprasRepository, CuentasComprasRepository>();
            services.AddScoped<IUnidadesMedidasRepository, UnidadesMedidasRepository>();
            services.AddScoped<IMarcasRepository, MarcasRepository>();

            services.AddScoped<ICajasRepository, CajasRepository>();
            //services.AddScoped<ICajasAperturasCierresRepository, CajasAperturasCierresRepository>();
            services.AddScoped<ICajasMovimientosRepository, CajasMovimientosRepository>();            
            services.AddScoped<ICajasTiposMovimientosRepository, CajasTiposMovimientosRepository>();

            services.AddScoped<IPresupuestosEstadosRepository, PresupuestosEstadosRepository>();
            services.AddScoped<IPresupuestosDescuentosRepository, PresupuestosDescuentosRepository>();
            services.AddScoped<IPresupuestosRepository, PresupuestosRepository>();


            services.AddScoped<IAlicuotasRepository, AlicuotasRepository>();
            services.AddScoped<IConceptosIncluidosRepository, ConceptosIncluidosRepository>();
            services.AddScoped<ITiposComprobantesRepository, TiposComprobantesRepository>();
            services.AddScoped<IComprobantesNumeracionesRepository, ComprobantesNumeracionesRepository>();
            services.AddScoped<ITiposResponsablesRepository, TiposResponsablesRepository>();
            services.AddScoped<IAlicuotasRepository, AlicuotasRepository>();

            services.AddScoped<IComprobantesRepository, ComprobantesRepository>();

            services.AddScoped<IEntidadesRepository, EntidadesRepository>();
            services.AddScoped<IFormasPagosRepository, FormasPagosRepository>();
            services.AddScoped<IFormasPagosCuotasRepository, FormasPagosCuotasRepository>();
            services.AddScoped<IFormasPagosCotizacionRepository, FormasPagosCotizacionRepository>();

            services.AddScoped<IConfiguracionesRepository, ConfiguracionesRepository>();
            services.AddScoped<ICombosRepository, CombosRepository>();

            services.AddScoped<IClientesCategoriasRepository, ClientesCategoriasRepository>();

            services.AddScoped<IProveedoresRepository, ProveedoresRepository>();

            services.AddScoped<IVentasRapidasRepository, VentasRapidasRepository>();

            services.AddScoped<IUserRolesRepository , UserRolesRepository>();

            services.AddScoped<IUserHelper, UserHelper>();
            services.AddScoped<IMailHelper, MailHelper>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var supportedCultures = new[] { "es-MX", "mx" };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseHttpsRedirection();
            //app.UseRouting();
            app.UseAuthentication();
            //app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
