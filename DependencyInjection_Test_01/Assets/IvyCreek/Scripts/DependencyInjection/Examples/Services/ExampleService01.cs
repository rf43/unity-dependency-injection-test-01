using UnityEngine;

namespace IvyCreek.Scripts.DependencyInjection.Examples.Services
{
    public class ExampleService01 : IExampleService
    {
        public void Initialize(string message = null)
        {
            Debug.Log($"{GetType().Name} initialized with message: {message}");
        }
    }
}