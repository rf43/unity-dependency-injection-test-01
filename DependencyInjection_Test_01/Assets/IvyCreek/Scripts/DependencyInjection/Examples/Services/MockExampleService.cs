using UnityEngine;

namespace IvyCreek.Scripts.DependencyInjection.Examples.Services
{
    public class MockExampleService : IExampleService
    {
        public void Initialize(string message = null)
        {
            Debug.Log($"{GetType().Name} initialized with message: {message}");
        }
    }
}