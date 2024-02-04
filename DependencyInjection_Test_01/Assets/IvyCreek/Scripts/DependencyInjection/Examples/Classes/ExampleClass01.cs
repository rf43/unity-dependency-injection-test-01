using IvyCreek.Scripts.DependencyInjection.Core;
using IvyCreek.Scripts.DependencyInjection.Examples.Services;
using UnityEngine;

namespace IvyCreek.Scripts.DependencyInjection.Examples.Classes
{
    /// <summary>
    /// This is a Unity MonoBehaviour class named ExampleClass01. It demonstrates the usage of dependency injection 
    /// through the Injector class. The class possesses a private instance of IExampleService interface. 
    /// It uses [Inject] decoration for the Init method to showcase dependency injection.
    /// </summary>
    public class ExampleClass01 : MonoBehaviour
    {
        // The private field of type IExampleService which is a dependency for this class
        private IExampleService _service;

        // The Inject attribute demonstrates the injection of dependency here.
        // The Init method is called with IExampleService as a parameter
        [Inject] 
        public void Init(IExampleService service)
        {
            _service = service;
            Debug.Log($"[Method Inject] {GetType().Name}.Init(IExampleService) called. {_service.GetType().Name} injected.");
        }
        
        // The Start method is a Unity callback method where the Initialize method of _service is being called.
        private void Start()
        {
            _service.Initialize($"[Method Inject] {_service.GetType().Name} initialized from {GetType().Name}");
        }
    }
}
