using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Okta.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
})
.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOktaMvc(new OktaMvcOptions
{
    //OktaDomain = builder.Configuration.GetValue<string>("Okta:OktaDomain"),
    //AuthorizationServerId = builder.Configuration.GetValue<string>("Okta:AuthorizationServerId"),
    //ClientId = builder.Configuration.GetValue<string>("Okta:ClientId"),
    //ClientSecret = builder.Configuration.GetValue<string>("Okta:ClientSecret"),
    OktaDomain = "https://dev-64683591.okta.com/",
    AuthorizationServerId = "default",
    ClientId = "0oa79etz7fpc2tayz5d7",
    ClientSecret = "BtfuxTZAyaCzgBL64izhUniJAgG6IBYBaPMRgXnU",
    Scope = new List<string> { "openid", "profile", "email", "phone"}
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
