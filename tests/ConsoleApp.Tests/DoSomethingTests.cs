using FluentAssertions;
using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;

namespace ConsoleApp.Tests;

public class DoSomethingTests
{
    [Theory, AutoMockData]
    public void ReturnHelloWorld_Should_ReturnHelloWorldString(string name)
    {
        var actualResult = DoSomething.ReturnHelloWorld(name);

        actualResult.Should().Be($"Hello {name}! Hello World!");
    }
}