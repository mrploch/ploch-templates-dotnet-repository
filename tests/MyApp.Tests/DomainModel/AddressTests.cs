using FluentAssertions;
using Ploch.Data.Model;
using Ploch.MyApp.DomainModel;
using Ploch.TestingSupport.XUnit3.AutoMoq;
using Xunit;

namespace Ploch.MyApp.Tests.DomainModel;

public class AddressTests
{
    [Theory, AutoMockData]
    public void Address_should_allow_setting_required_fields(string street1, string city, string postalCode, string country)
    {
        var address = new Address
        {
            Street1 = street1,
            City = city,
            PostalCode = postalCode,
            Country = country,
            PersonId = 5
        };

        address.Street1.Should().Be(street1);
        address.City.Should().Be(city);
        address.PostalCode.Should().Be(postalCode);
        address.Country.Should().Be(country);
        address.PersonId.Should().Be(5);
    }

    [Theory, AutoMockData]
    public void Address_should_allow_setting_optional_fields(string street2, string state)
    {
        var address = new Address
        {
            Street2 = street2,
            State = state
        };

        address.Street2.Should().Be(street2);
        address.State.Should().Be(state);
    }

    [Fact]
    public void Address_should_have_null_optional_fields_by_default()
    {
        var address = new Address();

        address.Street2.Should().BeNull();
        address.State.Should().BeNull();
    }

    [Fact]
    public void Address_should_implement_IHasId()
    {
        var address = new Address { Id = 99 };

        IHasId<int> hasId = address;
        hasId.Id.Should().Be(99);
    }
}
