using ConsoleApp;
using FluentAssertions;
using Ploch.TestingSupport.XUnit3.AutoMoq;
using Xunit;

namespace Ploch.MyApp.Tests;

public class MyAppTests
{
    [Theory, AutoMockData]
    public void MyAppService_should_add_two_numbers(double num1, double num2)
    {
        var result = MyAppService.Add(num1, num2);

        result.Should().Be(num1 + num2);
    }
}
