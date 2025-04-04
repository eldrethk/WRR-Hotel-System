using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WRR8_0.Data;
using WRRManagement.Domain.Hotels;
using WRRManagement.Domain.Packages;
using WRRManagement.Domain.RoomTypes;
using WRRManagement.Domain.System;
using WRRManagement.Infrastructure.HotelRepository;
using WRRManagement.Infrastructure.PackageRepository;
using WRRManagement.Infrastructure.RoomTypeRespository;
using WRRManagement.Infrastructure.SystemRepository;
using WRRManagement.Infrastructure;
using WRRManagement.Infrastructure.Data;
using WRRManagement.Domain.Amenities;
using WRRManagement.Infrastructure.AmenityRepository;
using Microsoft.AspNetCore.Builder;
using WRR8_0;
using WRR8_0.Components;
using Smart.Blazor;
using Microsoft.Extensions.Logging;
using WRRManagement.Domain.Reservation;
using WRRManagement.Infrastructure.ReservationRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("WRRDBConnection") ?? throw new InvalidOperationException("Connection string was not found.");
builder.Services.AddDbContext<WRRDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddAuthentication();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<WRRDbContext>();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddControllersWithViews();
builder.Services.AddSmart();
builder.Services.AddBlazorBootstrap();

builder.Services.AddHttpClient("DefaultClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7149/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("DefaultClient"));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();
// Remove or comment out the following line as 'AddFile' is not a valid method for ILoggingBuilder
// builder.Logging.AddFile(options =>
// {
//     options.FileName = "Logs/myapp-{Date}.txt";
// });

builder.Services.AddScoped<WRRContext>();
builder.Services.AddScoped<IRoomType, RoomTypeRepository>();
builder.Services.AddScoped<IRoomImage, RoomImageRepository>();
builder.Services.AddScoped<IRoomFeatures, RoomFeatureRepository>();
builder.Services.AddScoped<IRackRate, RackRateRespository>();
builder.Services.AddScoped<IHotelSystem, HotelSystemRepository>();
builder.Services.AddScoped<IAdultBase, AdultBaseRepository>();
builder.Services.AddScoped<IMaxBase, MaxBaseRepository>();
builder.Services.AddScoped<IHotel, HotelRepository>();
builder.Services.AddScoped<IHotelUser, HotelUserRepository>();
builder.Services.AddScoped<IDisclaimer, DisclaimerRepository>();
builder.Services.AddScoped<IOptInEmails, MarketingRepository>();
builder.Services.AddScoped<IPackage, PackageRepository>();
builder.Services.AddScoped<IPackageAmenity, PackageAmenityRepository>();
builder.Services.AddScoped<IPackageAllocation, PackageAllocationRepository>();
builder.Services.AddScoped<IPackageRate, PackageRateRepository>();
builder.Services.AddScoped<IPackageTierLevel, PackageTierLevelRepository>();
builder.Services.AddScoped<IExtraAmenity, AmenityRepository>();
builder.Services.AddScoped<IRoomAllocation, RoomAllocationRepository>();
builder.Services.AddScoped<IMinStay, MinStayRepository>();
builder.Services.AddScoped<ITierLevel, TierLevelRepository>();
builder.Services.AddScoped<IAvailableRackRoom, AvailablityRackRoomRepository>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.UseAntiforgery();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
