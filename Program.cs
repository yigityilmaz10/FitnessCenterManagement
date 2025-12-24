// Dosya: Program.cs
using FitnessCenterManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -------------------- DATABASE -----------------------
// appsettings.json dosyasından bağlantı dizesini okur.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// -------------------- IDENTITY ------------------------
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    // Şifre kurallarını gevşetiyoruz (Ödev için kolaylık)
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
})
.AddRoles<IdentityRole>() // 🔥 Rol Yönetimini etkinleştirir
.AddEntityFrameworkStores<ApplicationDbContext>();

// -------------------- MVC -----------------------------
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// ⬇️ SEED DATA VE ROL BAŞLATMA KRİTİK ALAN
using (var scope = app.Services.CreateScope())
{
    // Uygulama başlarken Admin rolünü ve kullanıcıyı ekler.
    await SeedData.Initialize(scope);
}
// ⬆️ SEED DATA BİTİŞ

// -------------------- PIPELINE ------------------------
if (app.Environment.IsDevelopment())
{
    // Geliştirme modunda detaylı hata ekranını gösterir (Hata tespiti için önemlidir!)
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization(); // Yetkilendirme Kontrollerini etkinleştirir

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();