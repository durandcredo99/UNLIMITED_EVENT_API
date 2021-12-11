using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UNLIMITED_EVENT_API.Helpers
{
    public class OpenApiOperationFilter : IOperationFilter
    {

        private static OpenApiSecurityScheme securitySchema = new OpenApiSecurityScheme
        {
            Description = "JJWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        };

        public static OpenApiSecurityScheme SecuritySchema
        {
            get { return securitySchema; }
        }



        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!context.MethodInfo.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute) &&
            !context.MethodInfo.DeclaringType.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute))
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            //securitySchema, new string[] { }
                            securitySchema, new string[] {"Bearer"}
                        }
                    }
                };
            }

        }
    }
}
