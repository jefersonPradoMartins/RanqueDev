using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using RanqueDev.Api.Configurations;
using RanqueDev.Infra.Context;
using RanqueDev.Infra.Repositories;
using RanqueDev.Data.EFCore.UoW;
using RanqueDev.Domain.Entities.Identity;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using RanqueDev.Infra.Interfaces.Repository;
using RanqueDev.Infra.Interfaces;
using RanqueDev.Api.Token;
using RanqueDev.Api.Extensions;
using Hellang.Middleware.ProblemDetails;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddEndpointsApiExplorer();

#region Swagger
builder.Services.AddSwaggerGen(sg =>
    {
        sg.SwaggerDoc("v1", new OpenApiInfo{ Title = "RanqueDev", Version = "v1" });

        sg.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {

            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization Header using Bearer scheme."
        });

        sg.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                }, new string[] {}
            }
        });
    });
#endregion
#region AutoMapper
builder.Services.AddAutoMapper(typeof(MapperConfig));
#endregion
#region Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          //builder.WithOrigins("http://localhost:4200")
                          builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});
#endregion
#region IdentityCore
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;



    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;
}).AddRoles<Role>()
            .AddEntityFrameworkStores<RanqueContext>()
            .AddRoleValidator<RoleValidator<Role>>()
            .AddRoleManager<RoleManager<Role>>()
            .AddSignInManager<SignInManager<User>>()
            .AddDefaultTokenProviders();
#endregion
#region Authentication
builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    }
);
builder.Services.AddMvc(o =>
{
    var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
    o.Filters.Add(new AuthorizeFilter(policy));
}).AddNewtonsoftJson();
#endregion
#region Context
builder.Services.AddDbContext<RanqueContext>();
#endregion
#region ProblemDetails
builder.Services.AddApiProblemDetails();
#endregion

builder.Services.AddTransient<ITagRepository, TagRepository>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IQuestaoRepository, QuestaoRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.Configure<DataProtectionTokenProviderOptions>(
    options=>options.TokenLifespan = TimeSpan.FromHours(2));
builder.Services.AddDataProtection().UseEphemeralDataProtectionProvider();


var app = builder.Build();
app.UseProblemDetails();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors(MyAllowSpecificOrigins);

app.Run();

