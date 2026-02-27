using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure; //Bu sınıf, uygulamanın servislerini merkezi bir yerde kaydetmek ve yönetmek
                          //için kullanılabilir. Genellikle Dependency Injection (DI) konteynerine
                          //servisleri eklemek için kullanılır. Bu sayede uygulamanın farklı
                          //bölümlerinde ihtiyaç duyulan servisler kolayca erişilebilir hale gelir.

//Infrastructure katmanındaki herşeyi bu class a (bu kutuya) topluyoruz
//static: C#ta .Net in var olan yapılarına sonradan özellik eklemek(extention method)
//için sınıfın static olması gerekir. 

//dependancy injection sınıfı,bunu program.cs e tek satırda ekleyeceğiz
public static class ServiceRegistration
{

    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));//program.csde uygulamaya
                                                                                                                                          //veritabanını tanıttıgımız kısmı
                                                                                                                                          //oradan silip bu class a ekledik
        services.AddScoped<IUnitOfWork, UnitOfWork>();
       
    }


}
