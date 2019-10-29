using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using Xtreem.Crusader.Utilities.Attributes;

namespace Xtreem.Crusader.Utilities.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ScanAssembly(this IServiceCollection services, bool scopedLifetime = true)
        {
            return services.Scan(s =>
            {
                AddRegistrations(s.FromEntryAssembly());
                AddRegistrations(s.FromApplicationDependencies(a => a.GetCustomAttribute<InjectAttribute>() != null));

                void AddRegistrations(IImplementationTypeSelector implementationTypeSelector)
                {
                    var concreteLifetimeSelector = implementationTypeSelector
                        .AddClasses(f => f.WithAttribute<InjectAttribute>().Where(t => !t.GetInterfaces().Any()))
                        .AsSelf();

                    var interfaceLifetimeSelector = implementationTypeSelector
                        .AddClasses(f => f.WithAttribute<InjectAttribute>(), false)
                        .AsImplementedInterfaces()
                        .UsingRegistrationStrategy(RegistrationStrategy.Replace(ReplacementBehavior.ImplementationType))
                        .AsMatchingInterface();

                    if (!scopedLifetime) return;
                    concreteLifetimeSelector.WithScopedLifetime();
                    interfaceLifetimeSelector.WithScopedLifetime();
                }
            });
        }
    }
}
