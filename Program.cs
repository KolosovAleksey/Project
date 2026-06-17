var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

// Маршрут для администрирования (чтобы работало /Admin/Events)
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/Events/{action=Index}/{id?}",
    defaults: new { controller = "AdminEvents" });

// Стандартный маршрут (для остальных контроллеров)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
