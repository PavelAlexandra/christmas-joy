using AutoMapper;
using ChristmasJoy.App.Helpers;
using ChristmasJoy.App.Services;
using ChristmasJoy.App.Models;
using ChristmasJoy.App.DbRepositories;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Mvc;

namespace ChristmasJoy.App
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
            #region ConfigureAuthentication
            var jwtAppSettingsOptions = Configuration.GetSection("JwtIssuerOptions");

            var SecretKey = jwtAppSettingsOptions["SecretKey"];
            SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };
      
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.GenericApiPolicy, policy => policy.RequireRole(Constants.GenericRole));
                options.AddPolicy(Constants.AdminApiPolicy, policy => policy.RequireRole(Constants.AdminRole));
            });
            #endregion

            #region ConfigureDI
            services.AddSingleton(new JwtIssuerOptions()
            {
              Issuer = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Issuer)],
              Audience = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Audience)],
              SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256)
            });

            var appConfig = new AppConfiguration(Configuration);
            services.AddSingleton<IAppConfiguration>(appConfig);
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ISignInService, SignInService>();
            services.AddScoped<IIdentityResolver, IdentityResolver>();
            services.AddScoped<IDocumentHelper, DocumentHelper>();
      #endregion

            #region EnableCord
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
              builder.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
            }));
            #endregion

            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddAutoMapper();
            services.Configure<MvcOptions>(options =>
            {
              options.Filters.Add(new CorsAuthorizationFilterFactory("MyPolicy"));
            });
      }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandler(
            builder =>
            {
                builder.Run(
                  async context =>
                  {
                      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                      context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                      var error = context.Features.Get<IExceptionHandlerFeature>();
                      if (error != null)
                      {
                          context.Response.AddApplicationError(error.Error.Message);
                          await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                      }
                  });
            });  
            
            app.UseAuthentication();

            //Redirect any non-API calls to the Angular application
            //so our application can handle the routing
            app.Use(async (context, next) =>
            {
                await next();

                if(context.Response.StatusCode == 404 && 
                   !Path.HasExtension(context.Request.Path.Value) &&
                   !context.Request.Path.Value.StartsWith("/api/"))
                {
                    context.Request.Path = "index.html";
                    await next();
                }
            });

            //Configure the app to server the index.html file from /wwwroot when accessing the server from a web browser
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCors("MyPolicy");

            //Configure the app for usage as API with default route at '/api/[Controller]'
            app.UseMvcWithDefaultRoute();

               
        }
    }
}
