using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal_VargasValeria.Data;
using ProyectoFinal_VargasValeria.Models;

var builder = WebApplication.CreateBuilder(args);

// Obtener cadena de conexi�n
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Configurar DbContext con Identity y las entidades personalizadas
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Aplicar migraciones autom�ticamente al iniciar
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Configurar Identity con roles
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // No requiere confirmaci�n de cuenta
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Agregar un servicio de email falso (puedes reemplazarlo con uno real como SendGrid)
builder.Services.AddSingleton<IEmailSender, FakeEmailSender>();

// Agregar autorizaci�n con pol�ticas personalizadas
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ViewDashboard", policy => policy.RequireClaim("Permission", "ViewDashboard"));
});

// Habilitar Razor Pages y controladores con vistas
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configurar el pipeline de la aplicaci�n
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware de seguridad y autenticaci�n
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


// Configurar rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Inicializar la base de datos con roles y usuarios predeterminados
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.InitializeAsync(services);
}

// Ejecutar la aplicaci�n
app.Run();
