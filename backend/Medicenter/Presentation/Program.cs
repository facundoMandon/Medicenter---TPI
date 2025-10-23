using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IAdministratorsRepository, AdministratorsRepository>();
builder.Services.AddScoped<IProfessionalsRepository, ProfessionalsRepository>();
builder.Services.AddScoped<IPatientsRepository, PatientsRepository>();
builder.Services.AddScoped<IHospitalsRepository, HospitalsRepository>();
builder.Services.AddScoped<IInsuranceRepository, InsuranceRepository>();
builder.Services.AddScoped<IAppointmentsRepository, AppointmentsRepository>();
builder.Services.AddScoped<ISpecialtiesRepository, SpecialtiesRepository>();



builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IAdministratorsService, AdministratorsService>();
builder.Services.AddScoped<IProfessionalsService, ProfessionalsService>();
builder.Services.AddScoped<IPatientsService, PatientsService>();
builder.Services.AddScoped<IHospitalsService, HospitalsService>();
builder.Services.AddScoped<IInsuranceService, InsuranceService>();
builder.Services.AddScoped<IAppointmentsService, AppointmentsService>();
builder.Services.AddScoped<ISpecialtiesService, SpecialtiesService>();

//Servicios de terceros (API)
builder.Services.AddScoped<IHolidaysService, HolidaysService>();

builder.Services.AddControllers();

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ?? Configuración de HttpClient para AbstractAPI Holidays
builder.Services.AddHttpClient(
    "HolidaysApi", 
    client =>
{
    client.BaseAddress = new Uri("https://holidays.abstractapi.com/v1/");
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

var app = builder.Build();

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
