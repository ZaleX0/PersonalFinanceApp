using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PersonalFinanceApp.Data;
using PersonalFinanceApp.Data.Entities;
using PersonalFinanceApp.Data.Interfaces;
using PersonalFinanceApp.Data.Repositories;
using PersonalFinanceApp.Services;
using PersonalFinanceApp.Services.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Repositories
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<Income>, Repository<Income>>();
builder.Services.AddScoped<IRepository<Expense>, Repository<Expense>>();
builder.Services.AddScoped<IRepository<IncomeCategory>, Repository<IncomeCategory>>();
builder.Services.AddScoped<IRepository<ExpenseCategory>, Repository<ExpenseCategory>>();

builder.Services.AddScoped<IFinanceUnitOfWork, FinanceUnitOfWork>();

// Service
builder.Services.AddScoped<FinanceSeeder>();
builder.Services.AddScoped<TestingService>();

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
app.Services.CreateScope()
    .ServiceProvider.GetRequiredService<FinanceSeeder>()
    .Seed();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
