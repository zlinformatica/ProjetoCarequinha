using ProjetoCarequinha.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<VideoService>();
//Construção (Builder) da manutenção da lista (aulas.json)
builder.Services.AddSingleton<VideoAulaService>();
builder.Services.AddSession();



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}


app.UseStaticFiles();
app.UseSession();

app.UseRouting();
app.MapControllerRoute(
    name: "admin",
    pattern: "admin/{action=Index}/{id?}",
    defaults: new { controller = "VideoAulaAdmin" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Videos}/{action=Index}/{id?}"
);

app.Run();
