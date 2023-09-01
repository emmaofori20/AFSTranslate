
using AFSTranslate;
using AFSTranslate.Models;
using AFSTranslate.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration;

configuration = builder.Configuration;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

//Add Authentication
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IAccessService, AccessService>();
builder.Services.AddScoped<ITranslationService, TranslationService>();

//DatabaseSeeder
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    DatabaseSeeder.SeedData( dbContext);
}

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Login}/{id?}");

app.Run();
