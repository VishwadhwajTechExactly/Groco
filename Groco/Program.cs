using Groco.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Groco.Utility;
using System.Runtime.InteropServices;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options
.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddRazorPages();
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
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
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using(var scope = app.Services.CreateScope())
{
    var userManager=scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    string email = "admin@admin.com";
    string password = "Admin@1234";
    if(await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser();
        user.UserName = email;
        user.Email = email;
        await userManager.CreateAsync(user,password);
        await userManager.AddToRoleAsync(user,SD.Role_Admin);
    }
}

app.Run();
