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
// Session eklemek için gerekli servisleri ekleme
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".NetCoreMvc.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum süresi
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IFoodModelStatePropCheck, FoodModelStatePropCheck>();
builder.Services.AddScoped<IUserModelStatePropCheck, UserModelStatePropCheck>();

builder.Services.AddMvc(options =>
{
    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => "Girilen deðer geçersiz.");
    options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => "Bu alanýn deðeri zorunludur.");
    options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "Bu alan gereklidir.");
    options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "Ýstek gövdesi gereklidir.");
    options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => "Girilen deðer geçersiz.");
    options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "Girilen deðer geçersiz.");
    options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "Girilen deðer bir sayý olmalýdýr.");
    options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(x => "Girilen deðer geçersiz.");
    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => "Girilen deðer geçersiz.");
    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => "Girilen deðer bir sayý olmalýdýr.");
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => "Bu alan boþ býrakýlamaz.");
});


builder.Services.AddControllersWithViews();

var app = builder.Build();






// HTTP isteði pipeline'ýný yapýlandýrma
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
