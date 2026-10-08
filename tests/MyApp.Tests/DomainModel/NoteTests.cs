using FluentAssertions;
using Ploch.Data.Model;
using Ploch.MyApp.DomainModel;
using Ploch.TestingSupport.XUnit3.AutoMoq;
using Xunit;

namespace Ploch.MyApp.Tests.DomainModel;

public class NoteTests
{
    [Theory, AutoMockData]
    public void Note_should_allow_setting_title_and_contents(string title, string contents)
    {
        var note = new Note
        {
            Title = title,
            Contents = contents,
            PersonId = 1
        };

        note.Title.Should().Be(title);
        note.Contents.Should().Be(contents);
        note.PersonId.Should().Be(1);
    }

    [Fact]
    public void Note_should_implement_IHasTitle()
    {
        var note = new Note { Title = "My Note" };

        IHasTitle hasTitle = note;
        hasTitle.Title.Should().Be("My Note");
    }

    [Fact]
    public void Note_should_implement_IHasContents()
    {
        var note = new Note { Contents = "Some content" };

        IHasContents hasContents = note;
        hasContents.Contents.Should().Be("Some content");
    }

    [Fact]
    public void Note_should_implement_IHasAuditProperties()
    {
        var now = DateTimeOffset.UtcNow;
        var note = new Note
        {
            CreatedTime = now,
            ModifiedTime = now,
            AccessedTime = now,
            CreatedBy = "author",
            LastModifiedBy = "editor",
            LastAccessedBy = "reader"
        };

        IHasAuditProperties audit = note;
        audit.CreatedTime.Should().Be(now);
        audit.CreatedBy.Should().Be("author");
        audit.LastModifiedBy.Should().Be("editor");
        audit.LastAccessedBy.Should().Be("reader");
    }

    [Fact]
    public void Note_should_initialize_tags_as_empty_and_categories_as_null()
    {
        var note = new Note();

        note.Tags.Should().BeEmpty();
        note.Categories.Should().BeNull();
    }
}
