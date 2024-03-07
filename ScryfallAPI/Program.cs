using ScryfallAPI;
using Microsoft.EntityFrameworkCore;
using ScryfallAPI.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using ScryfallAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ScryfallContext>(opt =>
    opt.UseSqlServer(connectionString: builder
                                        .Configuration
                                        .GetConnectionString("ScryFallConnection")
	));
builder.Services.AddHttpClient<APIRetriever>();
builder.Services.AddRazorPages();
builder.WebHost.UseStaticWebAssets();

builder.Services.AddControllers();
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

builder.Services.AddAuthorizationBuilder()
      .AddPolicy("AdminPolicy", policy =>
      {
          policy
                .RequireClaim(ClaimTypes.Role, "Administrator");
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
    return await context.Favorites.ToListAsync();
}).RequireAuthorization("AdminPolicy");

app.MapGet("cards/{id}", async (int id, ScryfallContext context) =>
{
    return await context.Favorites.Where(f => f.Id == id).ToListAsync();
});

app.MapPost("cards", async (FavoriteCards favorite, ScryfallContext context) =>
{
    await context.Favorites.AddAsync(favorite);
    await context.SaveChangesAsync();

    return Results.Created();
});

app.MapDelete("cards/{id}", async (int id, ScryfallContext context) =>
{
    var found = await context.Favorites.FindAsync(id);

    if (found is not null)
    {
        context.Favorites.Remove(found);
        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NoContent();
});

app.MapPut("cards/{id}", async (int id, FavoriteCards favorite , ScryfallContext context) =>
{
    var found = await context.Favorites.FindAsync(id);
    
    if(found is not null)
    {
        found.Name = favorite.Name;
        found.PennyRank = favorite.PennyRank;
        found.ReleasedAt = favorite.ReleasedAt;

        await context.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NoContent();

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
