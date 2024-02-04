using UnityEngine;

namespace IvyCreek.Scripts.DependencyInjection.Examples.Services
{
    public class ConcreteExampleService : IExampleService
    {
        public void Initialize(string message = null)
        {
            Debug.Log($"{GetType().Name} initialized with message: {message}");
        }
    }
}