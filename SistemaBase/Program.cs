using SistemaBase.Helpers;
using Microsoft.EntityFrameworkCore;
using SistemaBase.Models;
using System.Configuration;
using Microsoft.AspNetCore.Identity;
using SistemaBase.Interface.Pdf;
using SistemaBase.Service.Pdf;
using Grand.Web.Common.ViewRender;
using Wkhtmltopdf.NetCore;
using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

var builder = WebApplication.CreateBuilder(args);

var cbrpySettings = builder.Configuration.GetSection("cbrpyAPISettings");

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IViewCreate, ViewCreate>();
builder.Services.AddWkhtmltopdf();

builder.Services.AddScoped<IViewRenderService, ViewRenderService>();
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
builder.Services.AddScoped<IGeneratePdf, GeneratePdf>();
builder.Services.AddScoped<IPdfService, HtmlToPdfService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
{
    config.AccessDeniedPath = "/Login/ErrorAcceso";
    config.LoginPath = "/Login/Index";
});


builder.Services.AddScoped<AutorizarUsuarioFilter>(provider =>
{
    // Obtener el contexto de la solicitud actual
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    var httpContext = httpContextAccessor.HttpContext;

    // Obtener el controlador, acción y otros valores de la ruta actual
    var routeData = httpContext?.GetRouteData();
    var controlador = routeData?.Values["controller"]?.ToString();
    var accion = routeData?.Values["action"]?.ToString();

    // Otros servicios que puedan ser necesarios
    var logger = provider.GetRequiredService<ILogger<AutorizarUsuarioFilter>>();
    var dbContext = provider.GetRequiredService<DbvinDbContext>();

    return new AutorizarUsuarioFilter(controlador, accion, logger, dbContext, httpContextAccessor);
});


builder.Services.AddControllers();
//var connectionString = builder.Configuration.GetConnectionString("AppDb");
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
builder.Services.AddDbContext<DbvinDbContext>(options => options.UseSqlServer("Server=146.190.125.170,1433; Database=db_restore;Persist Security Info=False;User ID=sa;Password=23Mlkakd##d;Encrypt=False;Trusted_Connection=False; MultipleActiveResultSets=true;"));
builder.Services.AddDbContext<DbPruebaContext>(options => options.UseSqlServer("Server=tcp:proecan.database.windows.net,1433;Initial Catalog=jekakuaapruebas;Persist Security Info=False;User ID=proecan;Password=Guardian.2391;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
builder.Services.AddDbContext<DbIdentificacionesContext>(options => options.UseSqlServer("Server=172.30.8.83,1433; Database=identificaciones;Persist Security Info=False;User ID=Conexion_Auxiliares_MyS;Password=Auxiliares_MyS;Encrypt=False;Trusted_Connection=False; MultipleActiveResultSets=true;"));
builder.Services.AddDbContext<DbAuxiliaresContext>(options => options.UseSqlServer("Server=172.30.8.83,1433; Database=sgjasu_ALE;Persist Security Info=False;User ID=Conexion_Auxiliares_MyS;Password=Auxiliares_MyS;Encrypt=False;Trusted_Connection=False; MultipleActiveResultSets=true;"));
builder.Services.AddDbContext<DbPercepcionesContext>(options => options.UseSqlServer("Server=172.30.8.22,1433; Database=Percepciones;Persist Security Info=False;User ID=Conexion_MyS;Password=Conexion_MyS;Encrypt=False;Trusted_Connection=False; MultipleActiveResultSets=true;"));
builder.Services.AddControllersWithViews(options => options.EnableEndpointRouting = false).AddSessionStateTempDataProvider();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ADMINISTRADORES", policy => policy.RequireRole("ADMIN"));
});

// Agregar la configuraci�n de HttpClientFactory
builder.Services.AddHttpClient("cbrpyClient", client =>
{
    client.BaseAddress = new Uri(cbrpySettings.GetValue<string>("BaseApiUrl")); 
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
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
app.UseAuthentication();    
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");
app.Run();
