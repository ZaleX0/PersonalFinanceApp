using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PersonalFinanceApp.Data;
using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Data.Interfaces;
using PersonalFinanceApp.Data.Repositories;
using PersonalFinanceApp.Services;
using PersonalFinanceApp.Services.Interfaces;
using PersonalFinanceApp.Services.Middleware;
using PersonalFinanceApp.Services.Models.User;
using PersonalFinanceApp.Services.Models.Validators;
using PersonalFinanceApp.Services.Seeders;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Auth
var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
    };
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Middleware
builder.Services.AddScoped<ErrorHandlingMiddleware>();

// Validators
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();

// Repositories
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<Income>, Repository<Income>>();
builder.Services.AddScoped<IRepository<Expense>, Repository<Expense>>();
builder.Services.AddScoped<IRepository<IncomeCategory>, Repository<IncomeCategory>>();
builder.Services.AddScoped<IRepository<ExpenseCategory>, Repository<ExpenseCategory>>();
builder.Services.AddScoped<IRepository<RegularIncome>, Repository<RegularIncome>>();
builder.Services.AddScoped<IRepository<RegularExpense>, Repository<RegularExpense>>();

builder.Services.AddScoped<IFinanceUnitOfWork, FinanceUnitOfWork>();

// Service
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IIncomesService, IncomesService>();
builder.Services.AddScoped<IExpensesService, ExpensesService>();
builder.Services.AddScoped<IIncomeCategoriesService, IncomeCategoriesService>();
builder.Services.AddScoped<IExpenseCategoriesService, ExpenseCategoriesService>();
builder.Services.AddScoped<IRegularIncomesService, RegularIncomesService>();
builder.Services.AddScoped<IRegularExpensesService, RegularExpensesService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<IIncomeExpenseService, IncomeExpenseService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<FinanceSeeder>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var openApiSecurityScheme = new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    };
    var openApiSecurityRequirement = new OpenApiSecurityRequirement
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
    };
    options.AddSecurityDefinition("Bearer", openApiSecurityScheme);
    options.AddSecurityRequirement(openApiSecurityRequirement);
});

var app = builder.Build();

// Seed Sample Data
await app
    .Services.CreateScope()
    .ServiceProvider.GetRequiredService<FinanceSeeder>()
    .SeedAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
