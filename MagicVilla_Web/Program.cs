using MagicVilla_Web;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(MappingConfig)); //Se agrega AutoMapper como servicio. Con typeof(MappingConfig) se indica que se deben buscar los perfiles de mapeo en esa clase o en las clases asociadas a ella

builder.Services.AddHttpClient<IVillaService, VillaService>(); //Se configura un cliente HTTP en el contenedor de dependencias, lo que permite inyectarlo en otras partes de la aplicación. El cliente HTTP se configurar+a para que funcione con IVillaService y utilice VillaService para realizar solicitudes HTTP.
builder.Services.AddScoped<IVillaService, VillaService>(); //AddScoped especifica que se crea una instancia nueva de VillaService por cada solicitud HTTP dentro del alcance de una solicitud HTTP. Esto asegura que cada solicitud tenga su propia instancia de VillaService y evita problemas de concurrencia o mezcla de datos entre solicitudes.

builder.Services.AddHttpClient<IVillaNumberService, VillaNumberService>(); //Se configura un cliente HTTP en el contenedor de dependencias, lo que permite inyectarlo en otras partes de la aplicación. El cliente HTTP se configurar+a para que funcione con IVillaService y utilice VillaService para realizar solicitudes HTTP.
builder.Services.AddScoped<IVillaNumberService, VillaNumberService>(); //AddScoped especifica que se crea una instancia nueva de VillaService por cada solicitud HTTP dentro del alcance de una solicitud HTTP. Esto asegura que cada solicitud tenga su propia instancia de VillaService y evita problemas de concurrencia o mezcla de datos entre solicitudes.

builder.Services.AddHttpClient<IUserService, UserService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                                    .AddCookie(options =>
                                    {
                                        options.Cookie.HttpOnly = true;
                                        options.ExpireTimeSpan = TimeSpan.FromMinutes(100);
                                        options.LoginPath = "/User/Login";
                                        options.AccessDeniedPath = "/User/AccessDenied";
                                        options.SlidingExpiration = true;
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

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
