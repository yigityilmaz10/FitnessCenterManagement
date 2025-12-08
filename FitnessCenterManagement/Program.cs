// Dosya: FitnessCenterManagement/Program.cs (GÜNCEL HALİ)

using FitnessCenterManagement.Data; // SeedData ve DbContext için
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -------------------- DATABASE -----------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// -------------------- IDENTITY ------------------------
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    // Şifre kurallarını gevşetiyoruz (Ödev için 3 karakter ve basitleştirilmiş)
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;
})
.AddRoles<IdentityRole>() // ⬅️ 1. DEĞİŞİKLİK: Rol Yönetimini etkinleştirir
.AddEntityFrameworkStores<ApplicationDbContext>();

// -------------------- MVC -----------------------------
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// ⬇️ 2. DEĞİŞİKLİK: Seed Data (Rol ve Admin Kullanıcı) Başlatma
using (var scope = app.Services.CreateScope())
{
    // SeedData.Initialize metodu çağrılıyor. 
    // Bu metod, Admin ve Member rollerini ve Admin kullanıcısını oluşturacak.
    await SeedData.Initialize(scope);
}
// ⬆️ Seed Data Bloğu Sonu

// -------------------- PIPELINE ------------------------
if (app.Environment.IsDevelopment())
{
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
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();