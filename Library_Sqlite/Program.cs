using FluentValidation;
using Library.AutoMappers;
using Library.DTOs;
using Library.Models;
using Library.Repository;
using Library.Services;
using Library.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

/* Configure the connection string globally so that it can be used throughout the entire application, we add support for SQL Server by 
indicating in the dependency injection that the context will use SQL Server with the following connection string */
// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("Connection");

builder.Services.AddDbContext<LibraryContext>(options =>
{
    options.UseSqlite(connectionString);
    // Disable tracking
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});


// Avoid circular references when using includes in controllers
builder.Services.AddControllers().AddJsonOptions(options =>
options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configure security
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(
                     Encoding.UTF8.GetBytes(builder.Configuration["JWTKey"]))
               });

// Setting up security in Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
        "JWT Authentication Using Bearer Scheme. \r\n\r " +
        "Enter the word 'Bearer' followed by a space and the authentication token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });

});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IPublishingHouseService, PublishingHouseService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ITokenService, TokenService>();


builder.Services.AddTransient<IManagerFiles, ManagerFiles>();
builder.Services.AddTransient<ActionsService>();
builder.Services.AddTransient<HashService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDataProtection();

// Validators
builder.Services.AddScoped<IValidator<AuthorInsertDTO>, AuthorInsertValidator>();
builder.Services.AddScoped<IValidator<AuthorUpdateDTO>, AuthorUpdateValidator>();
builder.Services.AddScoped<IValidator<PublishingHouseInsertDTO>, PublishingHouseInsertValidator>();
builder.Services.AddScoped<IValidator<PublishingHouseUpdateDTO>, PublishingHouseUpdateValidator>();
builder.Services.AddScoped<IValidator<BookInsertDTO>, BookInsertValidator>();
builder.Services.AddScoped<IValidator<BookUpdateDTO>, BookUpdateValidator>();

// Repository
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IPublishingHouseRepository, PublishingHouseRepository>();

// Mappers
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
