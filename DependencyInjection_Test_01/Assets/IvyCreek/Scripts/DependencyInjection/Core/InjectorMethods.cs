using System;
using System.Linq;

namespace IvyCreek.Scripts.DependencyInjection.Core
{
    public partial class Injector
    {
        /// <summary>
        /// Injects dependencies into the methods of the given instance.
        /// </summary>
        /// <param name="instance">The instance to inject dependencies into.</param>
        /// <param name="type">The type of the instance.</param>
        /// <exception cref="Exception">Thrown if failed to inject dependencies into a method.</exception>
        private void InjectMethods(object instance, Type type)
        {
            // Inject into methods
            var injectableMethods = type.GetMethods(KBindingFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectableMethod in injectableMethods)
            {
                var requiredParameters = injectableMethod.GetParameters()
                    .Select(parameter => parameter.ParameterType)
                    .ToArray();
                var resolvedInstances = requiredParameters.Select(Resolve).ToArray();
                if (resolvedInstances.Any(resolvedInstance => resolvedInstance == null))
                {
                    throw new Exception(
                        $"Failed to inject dependencies into method '{injectableMethod.Name}' of class '{type.Name}'.");
                }

                injectableMethod.Invoke(instance, resolvedInstances);
            }
        }
    }
}