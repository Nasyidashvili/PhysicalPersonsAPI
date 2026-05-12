using Microsoft.EntityFrameworkCore;
using PhysicalPersonsAPI.Data;
using PhysicalPersonsAPI.Filters;
using PhysicalPersonsAPI.Interfaces;
using PhysicalPersonsAPI.Repositories.Implementations;
using PhysicalPersonsAPI.Repositories.Interfaces;
using PhysicalPersonsAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelAttribute>();
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet    /openapi
builder.Services.AddOpenApi();


builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IPhysicalPersonService, PhysicalPersonService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPhoneNumberRepository, PhoneNumberRepository>();
builder.Services.AddScoped<IRelatedPersonRepository, RelatedPersonRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLocalization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();
app.UseMiddleware<PhysicalPersonsAPI.Middleware.ErrorHandlingMiddleware>();
app.UseMiddleware<PhysicalPersonsAPI.Middleware.LanguageMiddleware>();

app.MapControllers();

app.Run();
