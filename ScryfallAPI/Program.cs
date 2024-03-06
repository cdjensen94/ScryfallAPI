using ScryfallAPI;
using Microsoft.EntityFrameworkCore;
using ScryfallAPI.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ScryfallContext>(opt => 
    opt.UseSqlServer(
    connectionString: "data source=LAPTOP-NMJCSUQD\\SQLEXPRESS;initial catalog=Scryfall;trusted_connection=true;TrustServerCertificate=True ")
    );
builder.Services.AddHttpClient<APIRetriever>();
builder.Services.AddRazorPages();
builder.WebHost.UseStaticWebAssets();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(opt =>
       {
           opt.ExpireTimeSpan = TimeSpan.FromMinutes(60);
           opt.Events = new CookieAuthenticationEvents()
           {
               OnRedirectToLogin = (context) =>
               {
                   context.HttpContext.Response.Redirect("https://localhost:7223/login");
                   return Task.CompletedTask;
               }
           };
       });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region CRUDs
app.MapGet("/cards", async (ScryfallContext context) =>
{
    await context.Favorites.ToListAsync();
 
});
#endregion

app.MapRazorPages();
app.UseDefaultFiles();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict
});

app.MapControllers();

app.Run();
