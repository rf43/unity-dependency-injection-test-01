using IvyCreek.Scripts.DependencyInjection.Core;
using IvyCreek.Scripts.DependencyInjection.Examples.Factories;
using IvyCreek.Scripts.DependencyInjection.Examples.Services;
using UnityEngine;

namespace IvyCreek.Scripts.DependencyInjection.Examples
{
    [Provides]
    public class ExampleProvider : MonoBehaviour
    {
        #region Concrete Factories

        // An example of how to use the [Provide] attribute to provide a factory.
        
        [Provide]
        public ExampleServiceFactory ProvideExampleServiceFactory()
        {
            Debug.Log("ProvideExampleServiceFactory => ExampleServiceFactory initialized from ExampleProvider");
            return new ExampleServiceFactory();
        }

        #endregion

        #region Interface Services

        // An example of how to use the [Provide] attribute to provide a service through an interface.
        // Another provided type, ExampleServiceFactory, is being passed as a parameter to the method.
        //
        // One thing to note here is that if a provided type is being passed as a parameter to another
        // provide method, the passed type must be provided before the method that is using it as a
        // parameter is declared. If this happens, an exception will be thrown.
        //
        // This is a limitation of the current implementation of the dependency injection system.
        // Hopefully, in the future, I can find a way to make this more flexible.
        [Provide]
        public IExampleService ProvideInterfaceExampleService(ExampleServiceFactory factory)
        {
            var service = factory.CreateService();
            service.Initialize(
                $"ProvideInterfaceExampleService => {service.GetType().Name} initialized from {GetType().Name}");
            return service;
        }

        #endregion

        #region Concrete Services

        // This is a simple example of how to use the [Provide] attribute to provide a service.
        // These services are providing concrete implementations of the IExampleService interface.

        [Provide]
        public ExampleService01 ProvideExampleService01()
        {
            var service = new ExampleService01();
            service.Initialize($"{service.GetType().Name} initialized from {GetType().Name}");
            return service;
        }

        [Provide]
        public ExampleService02 ProvideExampleService02()
        {
            var service = new ExampleService02();
            service.Initialize($"{service.GetType().Name} initialized from {GetType().Name}");
            return service;
        }

        #endregion
    }
}