using DocumentApprovalSystem.Web.Models;
using DocumentApprovalSystem.Web;
using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System;
using DocumentApprovalSystem.Data;
using wCyber.Helpers.Identity.Auth;
using DocumentApprovalSystem.Lib;
using Microsoft.EntityFrameworkCore;
using DinkToPdf.Contracts;
using DinkToPdf;
using wCyber.Lib.FileStorage.Azure;
using wCyber.Lib.FileStorage;

var builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment environment = builder.Environment;

// Add services to the container.
builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("SchoolDbConn");
builder.Services.AddDbContext<DocumentApprovalDbContext>(options => options.UseNpgsql(connectionString));

if (builder.Configuration["DocunentApproval:StorageAcc:Uri"] != null)
{
    builder.Services.AddSingleton<IFileStore>(new AzureBlobStore(builder.Configuration["DocunentApproval:StorageAcc:Uri"], builder.Configuration["SchoolMgt:StorageAcc:Name"], builder.Configuration["SchoolMgt:StorageAcc:Key"]));
}
else builder.Services.AddSingleton<IFileStore>(new FSFileStore("./wwwroot/webdata"));

builder.Services.Configure<UserRoleTypeOptions>(o =>
{
    o.RoleType = typeof(UserRole);
});

builder.Services.AddDefaultIdentity<DocumentApprovalSystem.Data.User>()
    .AddUserStore<SysUserStore<DocumentApprovalSystem.Data.User, DocumentApprovalDbContext>>()
    .AddClaimsPrincipalFactory<SysUserStore<DocumentApprovalSystem.Data.User, DocumentApprovalDbContext>>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(o =>
{
    o.ExpireTimeSpan = TimeSpan.FromDays(15);
    o.SlidingExpiration = true;
    o.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
    {
        OnSigningIn = async context => await context.InitUser()
    };
});
builder.Services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.UserNameClaimType = ClaimTypes.Name;
    options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
    options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
});

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
System.Runtime.Loader.AssemblyLoadContext.Default.ResolvingUnmanagedDll += (o, e) =>
{
    var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";
    var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    var wkHtmlToPdfPath = Path.Combine(environment.ContentRootPath, $"wkhtmltox/v0.12.4/{architectureFolder}/libwkhtmltox.{(isWindows ? "dll" : "so")}");
    CustomAssemblyLoader context = new();
    return context.LoadUnmanagedLibrary(wkHtmlToPdfPath);
};
var converter = new SynchronizedConverter(new PdfTools());
builder.Services.AddSingleton(typeof(IConverter), converter);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
