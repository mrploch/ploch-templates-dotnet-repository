namespace Ploch.TemplateLibrary;

/// <summary>
///     Normalises free-form text into a predictable, single-line form.
/// </summary>
/// <remarks>
///     This type is a placeholder that shows the repository conventions: XML documentation on every public member,
///     argument validation and a matching test class. Replace it with the library's real API.
/// </remarks>
/// <example>
///     <code>
///     var slug = TextNormalizer.ToSlug("  Hello,   World!  ");
///     // slug == "hello-world"
///     </code>
/// </example>
public static class TextNormalizer
{
    /// <summary>
    ///     Collapses every run of whitespace in <paramref name="text" /> into a single space and trims both ends.
    /// </summary>
    /// <param name="text">The text to normalise.</param>
    /// <returns>The normalised text, or an empty string when <paramref name="text" /> contains only whitespace.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="text" /> is <see langword="null" />.</exception>
    public static string CollapseWhitespace(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        return string.Join(' ', text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
    }

    /// <summary>
    ///     Converts <paramref name="text" /> into a lower-case, hyphen-separated slug containing only letters and digits.
    /// </summary>
    /// <param name="text">The text to convert.</param>
    /// <returns>The slug, or an empty string when <paramref name="text" /> contains no letters or digits.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="text" /> is <see langword="null" />.</exception>
    public static string ToSlug(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        var words = new List<string>();
        var current = new System.Text.StringBuilder();

        foreach (var character in text)
        {
            if (char.IsLetterOrDigit(character))
            {
                current.Append(char.ToLowerInvariant(character));
            }
            else if (current.Length > 0)
            {
                words.Add(current.ToString());
                current.Clear();
            }
        }

        if (current.Length > 0)
        {
            words.Add(current.ToString());
        }

        return string.Join('-', words);
    }
}
