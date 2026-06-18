using EventPortal.Data;
using EventPortal.Entities;
using EventPortal.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Подключение SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=EventPortal.db"));

// Identity с ApplicationUser и ролями
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Настройка cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Регистрация сервисов
builder.Services.AddScoped<IRegistrationService, RegistrationService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Инициализация базы данных
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    DbInitializer.Initialize(dbContext, userManager, roleManager);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Маршрут администрирования
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{controller=AdminEvents}/{action=Index}/{id?}");

// Основной маршрут
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Events}/{action=Index}/{id?}");

app.Run();
