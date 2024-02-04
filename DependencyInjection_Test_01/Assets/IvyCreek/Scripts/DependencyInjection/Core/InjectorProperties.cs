using System;
using System.Linq;

namespace IvyCreek.Scripts.DependencyInjection.Core
{
    public partial class Injector
    {
        /// <summary>
        /// Injects dependencies into the properties of the given instance.
        /// </summary>
        /// <param name="instance">The instance to inject dependencies into.</param>
        /// <param name="type">The type of the instance.</param>
        /// <exception cref="Exception">Thrown if failed to inject dependencies into a property.</exception>
        private void InjectProperties(object instance, Type type)
        {
            // Inject into properties
            var injectableProperties = type.GetProperties(KBindingFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
            foreach (var injectableProperty in injectableProperties)
            {
                var propertyType = injectableProperty.PropertyType;
                var resolvedInstance = Resolve(propertyType);
                if (resolvedInstance == null)
                {
                    throw new Exception(
                        $"Failed to inject dependency into property '{injectableProperty.Name}' of class '{type.Name}'.");
                }

                injectableProperty.SetValue(instance, resolvedInstance);
            }
        }
    }
}