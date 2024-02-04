using IvyCreek.Scripts.DependencyInjection.Examples.Services;

namespace IvyCreek.Scripts.DependencyInjection.Examples.Factories
{
    /// <summary>
    /// Represents a factory for creating instances of <see cref="IExampleService"/>.
    /// </summary>
    public class ExampleServiceFactory
    {
        // The cached service instance.
        private IExampleService _cachedService;
        
        // Setting this to true will use the mock service, setting it to false will use the concrete service.
        private const bool UseMockService = false;

        /// <summary>
        /// Creates an instance of the example service.
        /// </summary>
        /// <returns>An instance of the example service.</returns>
        public IExampleService CreateService()
        {
            // If the service has not been created yet, create it.
            // The ??= operator is a shorthand for checking if the left-hand side
            // is null, and if it is, assigning the right-hand side to it.
            return _cachedService ??= UseMockService ? new MockExampleService() : new ConcreteExampleService();
        }
    }
}