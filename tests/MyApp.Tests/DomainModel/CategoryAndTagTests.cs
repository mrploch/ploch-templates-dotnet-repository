using FluentAssertions;
using Ploch.Data.Model;
using Ploch.Data.Model.CommonTypes;
using Ploch.MyApp.DomainModel;
using Xunit;

namespace Ploch.MyApp.Tests.DomainModel;

public class CategoryAndTagTests
{
    [Fact]
    public void PersonCategory_should_inherit_from_Category()
    {
        var category = new PersonCategory { Id = 1, Name = "Family" };

        category.Should().BeAssignableTo<Category<PersonCategory>>();
        category.Id.Should().Be(1);
        category.Name.Should().Be("Family");
    }

    [Fact]
    public void PersonCategory_should_support_hierarchy()
    {
        var parent = new PersonCategory { Id = 1, Name = "Contacts" };
        var child = new PersonCategory { Id = 2, Name = "Close Friends", Parent = parent };
        parent.Children = new List<PersonCategory> { child };

        child.Parent.Should().BeSameAs(parent);
        parent.Children.Should().Contain(child);
    }

    [Fact]
    public void PersonTag_should_inherit_from_Tag()
    {
        var tag = new PersonTag { Id = 1, Name = "VIP", Description = "Important person" };

        tag.Should().BeAssignableTo<Tag>();
        tag.Id.Should().Be(1);
        tag.Name.Should().Be("VIP");
        tag.Description.Should().Be("Important person");
    }

    [Fact]
    public void NoteCategory_should_inherit_from_Category()
    {
        var category = new NoteCategory { Id = 1, Name = "Meeting Notes" };

        category.Should().BeAssignableTo<Category<NoteCategory>>();
        category.Id.Should().Be(1);
        category.Name.Should().Be("Meeting Notes");
    }

    [Fact]
    public void NoteCategory_should_support_hierarchy()
    {
        var parent = new NoteCategory { Id = 1, Name = "Work" };
        var child = new NoteCategory { Id = 2, Name = "Projects", Parent = parent };
        parent.Children = new List<NoteCategory> { child };

        child.Parent.Should().BeSameAs(parent);
        parent.Children.Should().Contain(child);
    }

    [Fact]
    public void NoteTag_should_inherit_from_Tag()
    {
        var tag = new NoteTag { Id = 1, Name = "Urgent" };

        tag.Should().BeAssignableTo<Tag>();
        tag.Name.Should().Be("Urgent");
    }

    [Fact]
    public void PersonCategory_should_have_persons_back_reference()
    {
        var category = new PersonCategory();
        var person = new Person { FirstName = "Test", LastName = "User" };
        category.Persons = new List<Person> { person };

        category.Persons.Should().Contain(person);
    }

    [Fact]
    public void NoteTag_should_have_notes_back_reference()
    {
        var tag = new NoteTag();
        var note = new Note { Title = "Test Note" };
        tag.Notes = new List<Note> { note };

        tag.Notes.Should().Contain(note);
    }

    [Fact]
    public void PersonCategory_should_implement_IHasId_and_INamed()
    {
        var category = new PersonCategory { Id = 10, Name = "Work" };

        IHasId<int> hasId = category;
        INamed named = category;

        hasId.Id.Should().Be(10);
        named.Name.Should().Be("Work");
    }
}
