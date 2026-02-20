using FluentAssertions;
using Ploch.TestingSupport.XUnit3.AutoMoq;

namespace ConsoleApp.Tests;

public class DoSomethingTests
{
    [Theory]
    [AutoMockData]
    public void ReturnHelloWorld_Should_ReturnHelloWorldString(string name)
    {
        var actualResult = DoSomething.ReturnHelloWorld(name);

        actualResult.Should().Be($"Hello {name}! Hello World!");
    }
}
