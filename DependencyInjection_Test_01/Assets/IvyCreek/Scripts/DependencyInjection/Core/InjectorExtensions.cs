using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace IvyCreek.Scripts.DependencyInjection.Core
{
    internal static class InjectorExtensions
    {
        /// <summary>
        /// Register the providers for the given MonoBehaviours.
        /// </summary>
        /// <param name="monoBehaviours">The IEnumerable of MonoBehaviours to register the providers for.</param>
        /// <param name="action">The action to perform on each registered MonoBehaviour.</param>
        /// <returns>The IEnumerable of MonoBehaviours after registering the providers.</returns>
        internal static IEnumerable<MonoBehaviour> RegisterProviders(
            this IEnumerable<MonoBehaviour> monoBehaviours,
            Action<MonoBehaviour> action)
        {
            var registerProviders = monoBehaviours as MonoBehaviour[] ?? monoBehaviours.ToArray();
            registerProviders.Where(mb => Attribute.IsDefined(mb.GetType(), typeof(ProvidesAttribute)))
                .ToList()
                .ForEach(action);

            return registerProviders;
        }

        /// <summary>
        /// Injects dependencies into the given MonoBehaviours.
        /// </summary>
        /// <param name="monoBehaviours">The IEnumerable of MonoBehaviours to inject dependencies into.</param>
        /// <param name="bindingFlags">The binding flags to use when retrieving members.</param>
        /// <param name="action">The action to perform on each MonoBehaviour after injecting dependencies.</param>
        internal static void InjectDependencies(
            this IEnumerable<MonoBehaviour> monoBehaviours,
            BindingFlags bindingFlags,
            Action<MonoBehaviour> action)
        {
            monoBehaviours.Where(mb => IsInjectable(mb, bindingFlags))
                .ToList()
                .ForEach(action);
        }

        #region Helpers

        /// <summary>
        /// Determines if a MonoBehaviour is injectable.
        /// </summary>
        /// <param name="mb">The MonoBehaviour to check.</param>
        /// <param name="bindingFlags">The binding flags to use when retrieving members.</param>
        /// <returns><c>true</c> if the MonoBehaviour is injectable; otherwise, <c>false</c>.</returns>
        private static bool IsInjectable(MonoBehaviour mb, BindingFlags bindingFlags)
        {
            var members = mb.GetType().GetMembers(bindingFlags);
            return members.Any(member => Attribute.IsDefined(member, typeof(InjectAttribute)));
        }

        #endregion
    }
}