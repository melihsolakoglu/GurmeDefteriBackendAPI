namespace GurmeDefteriWebUI.Helpers
{
    using GurmeDefteriWebUI.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceLocator
    {
        private static readonly ServiceProvider _serviceProvider;

        static ServiceLocator()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // İhtiyaç duyulan tüm servisleri burada ekle
            services.AddHttpContextAccessor(); // HttpContextAccessor'ı ekleyin
            services.AddScoped<SessionService>(); // SessionService'i ekleyin
            services.AddControllersWithViews();
            // Diğer servisler buraya eklenebilir
        }

        public static T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }

}
