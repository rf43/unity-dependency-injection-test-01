using System;
using System.Linq;
using UnityEngine;

namespace IvyCreek.Scripts.DependencyInjection.Core
{
    public partial class Injector
    {
        /// <summary>
        /// Injects dependencies into the fields of the given instance.
        /// </summary>
        /// <param name="instance">The instance to inject dependencies into.</param>
        /// <param name="type">The type of the instance.</param>
        /// <exception cref="System.Exception">Thrown if failed to inject dependency into a field.</exception>
        private void InjectFields(object instance, Type type)
        {
            // Inject into fields
            var injectableFields = type.GetFields(KBindingFlags)
                .Where(member => Attribute.IsDefined(member, typeof(InjectAttribute)));

            foreach (var injectableField in injectableFields)
            {
                if (injectableField.GetValue(instance) != null)
                {
                    Debug.LogWarning(
                        $"[Injector] Field '{injectableField.Name}' of class '{type.Name}' is already set.");
                    continue;
                }
                

                var fieldType = injectableField.FieldType;
                var resolvedInstance = Resolve(fieldType);
                if (resolvedInstance == null)
                {
                    throw new Exception(
                        $"Failed to inject dependency into field '{injectableField.Name}' of class '{type.Name}'.");
                }

                injectableField.SetValue(instance, resolvedInstance);
            }
        }
    }
}