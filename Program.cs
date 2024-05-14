using System;
using System.Text;
using API.Helpers;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

// ASP Identity For JWT Token

var builder = WebApplication.CreateBuilder(args);

//Get Value for AppSettings:Token
var configValue = builder.Configuration.GetValue<string>("AppSettings:Token");

IdentityBuilder Builder =builder.Services.AddIdentityCore<User>(
    Option =>
    {
        Option.Password.RequireDigit = false;
        Option.Password.RequiredLength = 4;
        Option.Password.RequireLowercase = false;
        Option.Password.RequireUppercase = false;
        Option.Password.RequiredUniqueChars = 0;
        Option.Password.RequireNonAlphanumeric = false;
    });

Builder = new IdentityBuilder(Builder.UserType, typeof(Role), Builder.Services);
Builder.AddEntityFrameworkStores<DataContext>();
Builder.AddRoleValidator<RoleValidator<Role>>();
Builder.AddRoleManager<RoleManager<Role>>();
Builder.AddSignInManager<SignInManager<User>>();

// Authorization For JWT Middelware
Builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(Options =>
{
    Options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configValue)),
        ValidateIssuer = false,
        ValidateAudience = false

    };
});

builder.Services.AddDbContext<DataContext>(
    options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"))
    );

builder.Services.AddControllers(options =>
    {
        var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        options.Filters.Add(new AuthorizeFilter(policy));
    }
);

//Enabling AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<CloudSettings>(builder.Configuration.GetSection("CloudSettings"));
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddEndpointsApiExplorer();
//To enable the repository pattern or add services to the container
builder.Services.AddScoped<IAuthRepo, AuthRepo>();
builder.Services.AddScoped<ISouqlyRepo, SouqlyRepo>();
builder.Services.AddScoped<IAdminRepo, AdminRepo>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<ISupplierRepo, SupplierRepo>();
builder.Services.AddScoped<IShippingRepo, ShippingRepo>();

//API Info
builder.Services.AddSwaggerGen(options => 
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ecommerce API",
        Version = "v1"
    });
});

//Cors Policy
builder.Services.AddCors();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce API v1"));
}

app.UseRouting();
app.UseCors(x => x.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<CartHub>("/cart");
});
app.Run();

