using GurmeDefteriWebUI.Helpers;
using GurmeDefteriWebUI.Services;
using GurmeDefteriWebUI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpContextAccessor(); 
builder.Services.AddScoped<SessionService>(); 
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.Name = "AdminCheck";
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = async context =>
        {
            if (context.Request.Path.StartsWithSegments("/Account/Login") && context.HttpContext.User.Identity.IsAuthenticated && context.HttpContext.User.IsInRole("Admin"))
            {
                context.Response.Redirect("/Home/Index"); 
            }
            else
            {
                context.Response.Redirect(context.RedirectUri);
            }
            await Task.CompletedTask;
        }
    };
}
    );
// Session eklemek i�in gerekli servisleri ekleme
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".NetCoreMvc.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum s�resi
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IFoodModelStatePropCheck, FoodModelStatePropCheck>();
builder.Services.AddScoped<IUserModelStatePropCheck, UserModelStatePropCheck>();

builder.Services.AddMvc(options =>
{
    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => "Girilen de�er ge�ersiz.");
    options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => "Bu alan�n de�eri zorunludur.");
    options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "Bu alan gereklidir.");
    options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "�stek g�vdesi gereklidir.");
    options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => "Girilen de�er ge�ersiz.");
    options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "Girilen de�er ge�ersiz.");
    options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "Girilen de�er bir say� olmal�d�r.");
    options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(x => "Girilen de�er ge�ersiz.");
    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => "Girilen de�er ge�ersiz.");
    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => "Girilen de�er bir say� olmal�d�r.");
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => "Bu alan bo� b�rak�lamaz.");
});


builder.Services.AddControllersWithViews();

var app = builder.Build();






// HTTP iste�i pipeline'�n� yap�land�rma
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
