using Microsoft.EntityFrameworkCore;
using System;
using TodoList_Manager.Database;

var builder = WebApplication.CreateBuilder(args);

// Adicione o contexto do banco de dados
builder.Services.AddDbContext<Config>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



// Add services to the container.
builder.Services.AddControllersWithViews();

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
    pattern: "{controller=Listar}/{action=Listagem}/{id?}");
// pattern: "{controller=Cadastro}/{action=Cadastro}/{id?}");

app.Run();
