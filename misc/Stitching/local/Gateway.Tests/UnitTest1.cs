using System.Threading.Tasks;
using Demo.Gateway;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Gateway.Tests
{
    public class Tests
    {
        private const string UsersQuery = "query { users { id, name } }";
        private IRequestExecutor _accountsSchemaExecutor;
        private IRequestExecutor _stitchedSchemaExecutor;

        [SetUp]
        public async Task Setup()
        {
            var serviceCollection = new ServiceCollection();
            var startup = new Startup();
            startup.ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            _accountsSchemaExecutor = await serviceProvider.GetRequestExecutorAsync(Startup.Accounts);
            _stitchedSchemaExecutor = await serviceProvider.GetRequestExecutorAsync();
        }

        [Test]
        public async Task QueryAgainstAccountSchema()
        {
            var executionResult = await _accountsSchemaExecutor.ExecuteAsync(UsersQuery);
            var resultJson = await executionResult.ToJsonAsync();
            TestContext.WriteLine(resultJson);
            Assert.That(executionResult.Errors?.Count, Is.Null.Or.EqualTo(0));
        }

        [Test]
        public async Task QueryAgainstStitchedSchema()
        {
            var executionResult = await _stitchedSchemaExecutor.ExecuteAsync(UsersQuery);
            var resultJson = await executionResult.ToJsonAsync();
            TestContext.WriteLine(resultJson);
            Assert.That(executionResult.Errors?.Count, Is.Null.Or.EqualTo(0));
        }
    }
}