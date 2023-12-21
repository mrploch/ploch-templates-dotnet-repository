using Xunit;
using FluentAssertions;
using ConsoleApp;

namespace ConsoleApp.Tests
{
    public class DoSomethingTests
    {
        [Fact]
        public void ReturnHelloWorld_Should_ReturnHelloWorldString()
        {
           string actualResult = DoSomething.ReturnHelloWorld();
           
           actualResult.Should().Be("Hello World!");
        }
    }
}