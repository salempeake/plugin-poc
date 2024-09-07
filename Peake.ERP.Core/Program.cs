using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Peake.ERP.Core.Data;
using Peake.ERP.Core.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Enable runtime compilation of razor pages.
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

// Load module assemblies dynamically
var moduleAssemblies = LoadModuleAssemblies();
foreach (var assembly in moduleAssemblies)
{
    // Add the assembly as an Application Part
    builder.Services.AddMvc().AddApplicationPart(assembly);
    
    // Discover and register services from modules
    var moduleTypes = assembly.GetTypes()
                              .Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

    foreach (var moduleType in moduleTypes)
    {
        // Create an instance of the module and register its services
        var module = (IModule)Activator.CreateInstance(moduleType);
        module.RegisterServices(builder.Services);
    }
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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

IEnumerable<Assembly> LoadModuleAssemblies()
{
    // Replace "Modules" with the path to your module assemblies
    var assemblyPaths = Directory.GetFiles("Modules", "*.dll", SearchOption.AllDirectories);
    foreach (var path in assemblyPaths)
    {
        yield return Assembly.LoadFrom(path);
    }
}
