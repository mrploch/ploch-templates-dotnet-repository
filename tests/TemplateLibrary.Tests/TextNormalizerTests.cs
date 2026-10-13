using AutoFixture.Xunit3;

namespace Ploch.TemplateLibrary.Tests;

public class TextNormalizerTests
{
    [Theory]
    [InlineData("  Hello,   World!  ", "Hello, World!")]
    [InlineData("one\ttwo\nthree", "one two three")]
    [InlineData("already clean", "already clean")]
    [InlineData("   ", "")]
    [InlineData("", "")]
    public void CollapseWhitespace_should_collapse_runs_of_whitespace_and_trim(string input, string expected)
    {
        TextNormalizer.CollapseWhitespace(input).Should().Be(expected);
    }

    [Fact]
    public void CollapseWhitespace_should_throw_when_text_is_null()
    {
        var act = () => TextNormalizer.CollapseWhitespace(null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("text");
    }

    [Theory]
    [InlineData("  Hello,   World!  ", "hello-world")]
    [InlineData("Release 2.0 -- notes", "release-2-0-notes")]
    [InlineData("ALL CAPS", "all-caps")]
    [InlineData("!!!", "")]
    [InlineData("", "")]
    [InlineData("Café au lait", "cafe-au-lait")]
    [InlineData("  Crème brûlée, 2 portions!  ", "creme-brulee-2-portions")]
    [InlineData("naïve—approach", "naive-approach")]
    [InlineData("Привет world", "world")]
    [InlineData("emoji \U0001F600 here", "emoji-here")]
    [InlineData("math \U0001D400 letter", "math-letter")]
    [InlineData("lone \uD83D high", "lone-high")]
    [InlineData("lone \uDE00 low", "lone-low")]
    [InlineData("cut\uD83D", "cut")]
    public void ToSlug_should_produce_lower_case_hyphen_separated_words(string input, string expected)
    {
        TextNormalizer.ToSlug(input).Should().Be(expected);
    }

    [Theory]
    [AutoData]
    public void ToSlug_should_only_contain_lower_case_letters_digits_and_hyphens(string input)
    {
        var slug = TextNormalizer.ToSlug(input);

        slug.Should().MatchRegex("^[a-z0-9]+(-[a-z0-9]+)*$");
    }

    [Fact]
    public void ToSlug_should_throw_when_text_is_null()
    {
        var act = () => TextNormalizer.ToSlug(null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("text");
    }
}
