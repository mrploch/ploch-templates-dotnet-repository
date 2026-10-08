using FluentAssertions;
using Ploch.Data.Model;
using Ploch.MyApp.DomainModel;
using Ploch.TestingSupport.XUnit3.AutoMoq;
using Xunit;

namespace Ploch.MyApp.Tests.DomainModel;

public class PersonTests
{
    [Theory, AutoMockData]
    public void Person_should_allow_setting_all_properties(string firstName, string lastName, string email, string phone)
    {
        var person = new Person
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phone,
            DateOfBirth = new DateOnly(1990, 5, 15)
        };

        person.FirstName.Should().Be(firstName);
        person.LastName.Should().Be(lastName);
        person.Email.Should().Be(email);
        person.PhoneNumber.Should().Be(phone);
        person.DateOfBirth.Should().Be(new DateOnly(1990, 5, 15));
    }

    [Fact]
    public void Person_should_initialize_collections_as_empty()
    {
        var person = new Person();

        person.Addresses.Should().BeEmpty();
        person.Notes.Should().BeEmpty();
        person.Tags.Should().BeEmpty();
        person.Categories.Should().BeNull();
    }

    [Fact]
    public void Person_should_implement_IHasAuditProperties()
    {
        var now = DateTimeOffset.UtcNow;
        var person = new Person
        {
            CreatedTime = now,
            ModifiedTime = now,
            AccessedTime = now,
            CreatedBy = "test-user",
            LastModifiedBy = "test-user",
            LastAccessedBy = "test-user"
        };

        IHasAuditProperties audit = person;
        audit.CreatedTime.Should().Be(now);
        audit.ModifiedTime.Should().Be(now);
        audit.AccessedTime.Should().Be(now);
        audit.CreatedBy.Should().Be("test-user");
        audit.LastModifiedBy.Should().Be("test-user");
        audit.LastAccessedBy.Should().Be("test-user");
    }

    [Fact]
    public void Person_should_implement_IHasId()
    {
        var person = new Person { Id = 42 };

        IHasId<int> hasId = person;
        hasId.Id.Should().Be(42);
    }

    [Fact]
    public void Person_should_implement_IHasCategories_and_IHasTags()
    {
        var person = new Person();

        IHasCategories<PersonCategory> hasCategories = person;
        IHasTags<PersonTag> hasTags = person;

        hasCategories.Categories.Should().BeNull();
        hasTags.Tags.Should().BeEmpty();
    }
}
