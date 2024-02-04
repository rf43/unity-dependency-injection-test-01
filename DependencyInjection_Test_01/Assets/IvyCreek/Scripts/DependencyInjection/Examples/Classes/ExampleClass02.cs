using IvyCreek.Scripts.DependencyInjection.Core;
using IvyCreek.Scripts.DependencyInjection.Examples.Factories;
using IvyCreek.Scripts.DependencyInjection.Examples.Services;
using UnityEngine;

namespace IvyCreek.Scripts.DependencyInjection.Examples.Classes
{
    /// <summary>
    /// ExampleClass02 is a MonoBehaviour class demonstrating the use of dependency injection on 
    /// different services and a factory. It makes use of the Injector class to inject instances 
    /// of ExampleService01, ExampleService02 and ExampleServiceFactory.
    /// </summary>
    public class ExampleClass02 : MonoBehaviour
    {
        // Dependency injected instances of ExampleService01 and ExampleService02
        [Inject] private ExampleService01 _service01;
        [Inject] private ExampleService02 _service02;

        // A reference to the ExampleService factory
        private ExampleServiceFactory _factory;

        /// <summary>
        /// This method initialises the ExampleServiceFactory instance by the injector.
        /// </summary>
        /// <param name="factory">Injected object of ExampleServiceFactory</param>
        [Inject]
        public void Init(ExampleServiceFactory factory)
        {
            _factory = factory;
            Debug.Log($"[Method Inject] {GetType().Name}.Init() called. {_factory.GetType().Name} injected.");
        }

        /// <summary>
        /// This method is called as part of Unity's routine and it initializes the services 
        /// injected into this class and also creates and initializes a service from the factory.
        /// </summary>
        private void Start()
        {
            _service01
                .Initialize($"[Field Inject] {_service01.GetType().Name} initialized from {GetType().Name}");
            _service02
                .Initialize($"[Field Inject] {_service02.GetType().Name} initialized from {GetType().Name}");
            _factory.CreateService()
                .Initialize($"[Method Inject] from {_factory.GetType().Name} in {GetType().Name}");
        }
    }
}