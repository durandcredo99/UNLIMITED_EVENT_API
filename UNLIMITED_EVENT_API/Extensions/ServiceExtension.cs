using Contracts;
using Entities;
using Entities.Helpers;
using Entities.Models;
using Entities.RequestFeatures;
using LoggerServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNLIMITED_EVENT_API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => 
                builder.WithOrigins("http://localhost:5768")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("X-Pagination"));

            });
        }


        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }


        public static void ConfigureSession(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }


        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }


        public static void ConfigureRepositoryContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["ConnectionStrings:DbConnection"];
            IServiceCollection serviceCollections = services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(connectionString));
        }


        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ISortHelper<AppUser>, SortHelper<AppUser>>();
            services.AddScoped<ISortHelper<Banner>, SortHelper<Banner>>();
            services.AddScoped<ISortHelper<Category>, SortHelper<Category>>();
            services.AddScoped<ISortHelper<CategoryBlog>, SortHelper<CategoryBlog>>();
            services.AddScoped<ISortHelper<Blog>, SortHelper<Blog>>();
            services.AddScoped<ISortHelper<Order>, SortHelper<Order>>();
            services.AddScoped<ISortHelper<Comment>, SortHelper<Comment>>();
            services.AddScoped<ISortHelper<SubCategory>, SortHelper<SubCategory>>();
            services.AddScoped<ISortHelper<Commercial>, SortHelper<Commercial>>();
            services.AddScoped<ISortHelper<Event>, SortHelper<Event>>();
            services.AddScoped<ISortHelper<Payment>, SortHelper<Payment>>();
            services.AddScoped<ISortHelper<Place>, SortHelper<Place>>();
            services.AddScoped<ISortHelper<Promote>, SortHelper<Promote>>();
            services.AddScoped<ISortHelper<PromoteEvent>, SortHelper<PromoteEvent>>();
            services.AddScoped<ISortHelper<AnnualRate>, SortHelper<AnnualRate>>();
            services.AddScoped<ISortHelper<Partner>, SortHelper<Partner>>();
            services.AddScoped<ISortHelper<Sponsor>, SortHelper<Sponsor>>();
            services.AddScoped<ISortHelper<Workstation>, SortHelper<Workstation>>();

            services.AddScoped<IPromoteRepository, PromoteRepository>();
            services.AddScoped<IPromoteEventRepository, PromoteEventRepository>();
            services.AddScoped<IAnnualRateRepository, AnnualRateRepository>();
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }


        public static void ConfigureCookieAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentity<AppUser, Workstation>(option =>
            {
                option.Password.RequireDigit = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequiredLength = 6;
                option.User.RequireUniqueEmail = true;
                option.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";
            }).AddEntityFrameworkStores<RepositoryContext>()
                        .AddDefaultTokenProviders();


            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => {
                options.Cookie.Name = config["AuthSettings:cookieName"];
            });
        }
        


        public static void ConfigureJWTAuthenticationService(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentity<AppUser, Workstation>(option =>
            {
                option.Password.RequireDigit = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequiredLength = 8;
                option.User.RequireUniqueEmail = true;
                option.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";
            }).AddEntityFrameworkStores<RepositoryContext>()
                        .AddDefaultTokenProviders();


            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,

                    ValidAudience = config["AuthSettings:Audience"],
                    ValidIssuer = config["AuthSettings:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["AuthSettings:Key"])),
                };
            });

        }


        public static void ConfigureClaimPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(option =>
            {
                option.AddPolicy("readCategoryPolicy", policy => policy.RequireClaim("readCategory"));
                option.AddPolicy("writeCategoryPolicy", policy => policy.RequireClaim("writeCategory"));
                option.AddPolicy("manageCategoryPolicy", policy => policy.RequireClaim("manageCategory"));

                option.AddPolicy("readAppUserPolicy", policy => policy.RequireClaim("readAppUser"));
                option.AddPolicy("writeAppUserPolicy", policy => policy.RequireClaim("writeAppUser"));
                option.AddPolicy("manageAppUserPolicy", policy => policy.RequireClaim("manageAppUser"));

                option.AddPolicy("readBranchPolicy", policy => policy.RequireClaim("readBranch"));
                option.AddPolicy("writeBranchPolicy", policy => policy.RequireClaim("writeBranch"));
                option.AddPolicy("manageBranchPolicy", policy => policy.RequireClaim("manageBranch"));

                option.AddPolicy("readBranchLevelPolicy", policy => policy.RequireClaim("readBranchLevel"));
                option.AddPolicy("writeBranchLevelPolicy", policy => policy.RequireClaim("writeBranchLevel"));
                option.AddPolicy("manageBranchLevelPolicy", policy => policy.RequireClaim("manageBranchLevel"));

                option.AddPolicy("readPersonalFilePolicy", policy => policy.RequireClaim("readPersonalFile"));
                option.AddPolicy("writePersonalFilePolicy", policy => policy.RequireClaim("writePersonalFile"));
                option.AddPolicy("managePersonalFilePolicy", policy => policy.RequireClaim("managePersonalFile"));

                option.AddPolicy("readPartnerPolicy", policy => policy.RequireClaim("readPartner"));
                option.AddPolicy("writePartnerPolicy", policy => policy.RequireClaim("writePartner"));
                option.AddPolicy("managePartnerPolicy", policy => policy.RequireClaim("managePartner"));

                option.AddPolicy("readRatePolicy", policy => policy.RequireClaim("readRate"));
                option.AddPolicy("writeRatePolicy", policy => policy.RequireClaim("writeRate"));
                option.AddPolicy("manageRatePolicy", policy => policy.RequireClaim("manageRate"));

                option.AddPolicy("readSponsorPolicy", policy => policy.RequireClaim("readSponsor"));
                option.AddPolicy("writeSponsorPolicy", policy => policy.RequireClaim("writeSponsor"));
                option.AddPolicy("manageSponsorPolicy", policy => policy.RequireClaim("manageSponsor"));

                option.AddPolicy("readPaymentPolicy", policy => policy.RequireClaim("readPayment"));
                option.AddPolicy("writePaymentPolicy", policy => policy.RequireClaim("writePayment"));
                option.AddPolicy("managePaymentPolicy", policy => policy.RequireClaim("managePayment"));

                option.AddPolicy("readPaymentTypePolicy", policy => policy.RequireClaim("readPaymentType"));
                option.AddPolicy("writePaymentTypePolicy", policy => policy.RequireClaim("writePaymentType"));
                option.AddPolicy("managePaymentTypePolicy", policy => policy.RequireClaim("managePaymentType"));

                option.AddPolicy("readBranchLevelRegistrationRequirementPolicy", policy => policy.RequireClaim("readBranchLevelRegistrationRequirement"));
                option.AddPolicy("writeBranchLevelRegistrationRequirementPolicy", policy => policy.RequireClaim("writeBranchLevelRegistrationRequirement"));
                option.AddPolicy("manageBranchLevelRegistrationRequirementPolicy", policy => policy.RequireClaim("manageBranchLevelRegistrationRequirement"));

                option.AddPolicy("readSubscriptionPolicy", policy => policy.RequireClaim("readSubscription"));
                option.AddPolicy("writeSubscriptionPolicy", policy => policy.RequireClaim("writeSubscription"));
                option.AddPolicy("manageSubscriptionPolicy", policy => policy.RequireClaim("manageSubscription"));

                option.AddPolicy("readSubscriptionLinePolicy", policy => policy.RequireClaim("readSubscriptionLine"));
                option.AddPolicy("writeSubscriptionLinePolicy", policy => policy.RequireClaim("writeSubscriptionLine"));
                option.AddPolicy("manageSubscriptionLinePolicy", policy => policy.RequireClaim("manageSubscriptionLine"));

                option.AddPolicy("readUniversityPolicy", policy => policy.RequireClaim("readUniversity"));
                option.AddPolicy("writeUniversityPolicy", policy => policy.RequireClaim("writeUniversity"));
                option.AddPolicy("manageUniversityPolicy", policy => policy.RequireClaim("manageUniversity"));

                option.AddPolicy("readWorkstationPolicy", policy => policy.RequireClaim("readWorkstation"));
                option.AddPolicy("writeWorkstationPolicy", policy => policy.RequireClaim("writeWorkstation"));
                option.AddPolicy("manageWorkstationPolicy", policy => policy.RequireClaim("manageWorkstation"));
            });
        }


        public static void ConfigureNewtonsoftJson(this IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false).AddNewtonsoftJson(opt => {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
        }

        public static void ConfigureMailService(this IServiceCollection services, IConfiguration config)
        {
            var emailConfig = config
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);


            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
        }
    }
}
