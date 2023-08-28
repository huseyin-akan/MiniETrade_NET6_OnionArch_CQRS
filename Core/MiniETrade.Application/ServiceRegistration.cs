using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MiniETrade.Application.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(typeof(ServiceRegistration));

            //Böylece aşağıdaki kodda demiş olduk ki, BaseBusinessRuless'un çocuğu olan tüm tipleri IoC'ye ekle.
            serviceCollection.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));
        }

        //Assembly'de yani bu projede verilen tipi arayıp, buluyor ve onun bir instance'ını IoC'ye ekliyor.
        public static IServiceCollection AddSubClassesOfType(this IServiceCollection services,
             Assembly assembly, Type type, Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
        {
            var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
            foreach (var item in types)
                if (addWithLifeCycle is null) services.AddScoped(item);
                else addWithLifeCycle(services, type);
            return services;
        }
    }
}
