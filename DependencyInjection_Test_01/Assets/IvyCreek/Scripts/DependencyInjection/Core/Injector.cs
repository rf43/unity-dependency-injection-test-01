using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace IvyCreek.Scripts.DependencyInjection.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
    public sealed class InjectAttribute : PropertyAttribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ProvideAttribute : PropertyAttribute { }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ProvidesAttribute : Attribute { }


    [DefaultExecutionOrder(-1000)]
    public partial class Injector : MonoBehaviour
    {
        // The BindingFlags used to find members
        private const BindingFlags KBindingFlags =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        // The registry of all dependencies
        private readonly Dictionary<Type, object> _registry = new();

        #region Unity Lifcycle

        private void Awake()
        {
            FindMonoBehaviours()
                .ToArray()
                .RegisterProviders(Register)
                .InjectDependencies(KBindingFlags, Inject);
        }

        #endregion

        private void Register<T>(T provider) where T : MonoBehaviour
        {
            var methods = provider.GetType().GetMethods(KBindingFlags);
            
            foreach (var method in methods)
            {
                if (!Attribute.IsDefined(method, typeof(ProvideAttribute))) continue;

                var returnType = method.ReturnType;

                // Get method parameters
                var parameters = method.GetParameters();
                var parameterValues = new object[parameters.Length];

                for (var i = 0; i < parameters.Length; i++)
                {
                    var paramType = parameters[i].ParameterType;

                    // Check if the required parameter type exists in the registry
                    if (_registry.TryGetValue(paramType, out var value))
                    {
                        parameterValues[i] = value;
                    }
                    else
                    {
                        // If a parameter type doesn't exist in the registry,
                        // throw an exception or handle this case accordingly
                        throw new Exception($"No provider for type '{paramType.Name}' found in the registry.");
                    }
                }


                // Now invoke the method with the arguments from the registry
                var providedInstance = method.Invoke(provider, parameterValues);
                if (providedInstance != null)
                {
                    _registry.Add(returnType, providedInstance);
                }
                else
                {
                    throw new Exception(
                        $"Provider method '{method.Name}' in class '{provider.GetType().Name}' returned null " +
                        $"when providing type '{returnType.Name}'.");
                }
            }
        }

        private void Inject(object instance)
        {
            var type = instance.GetType();
            InjectFields(instance, type);
            InjectMethods(instance, type);
            InjectProperties(instance, type);
        }

        private object Resolve(Type type)
        {
            _registry.TryGetValue(type, out var resolvedInstance);
            return resolvedInstance;
        }

        private static IEnumerable<MonoBehaviour> FindMonoBehaviours()
        {
            return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
        }
    }
}