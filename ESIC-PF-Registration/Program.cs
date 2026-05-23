using Insfrastructure.DbModels;
using Microsoft.EntityFrameworkCore;
using Repository.District;
using Repository.Employee;
using Repository.State;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EsicPfRegistrationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEmployee, Services.Employee.DALClass>();
builder.Services.AddScoped<IState, Services.States.DALClass>();
builder.Services.AddScoped<IDistrict, Services.District.DALClass>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    // pattern: "{controller=Home}/{action=Index}/{id?}");
    pattern: "{controller=Employee}/{action=CreateEmployeeReg}/{id?}");

app.Run();
