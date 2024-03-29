using IdentityNetCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDBContext>(o => o.UseSqlServer("Data Source=.;Initial Catalog=Identity;Integrated Security=True"));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>();
builder.Services.Configure<IdentityOptions>(o =>
{
    o.Password.RequiredLength = 3;
    o.Password.RequireDigit = true;
    o.Password.RequireNonAlphanumeric = false;

    o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    o.Lockout.MaxFailedAccessAttempts = 3;

});

builder.Services.ConfigureApplicationCookie(o =>
{
    o.LoginPath = "/Identity/Signin";
    o.AccessDeniedPath = "/Identity/AccessDenied";
    o.ExpireTimeSpan = TimeSpan.FromMinutes(60);
});

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidIssuer = "http://localhost",
        ValidAudience = "http://localhost",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0123456789ASDFGH"))
    };
});

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("DepAdmin", p =>
    {
        p.RequireClaim("Department", "IT").RequireRole("admin");
    });
});

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
