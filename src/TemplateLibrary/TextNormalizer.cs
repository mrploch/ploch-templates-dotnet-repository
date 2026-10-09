using System.Globalization;
using System.Text;

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
///     var slug = TextNormalizer.ToSlug("  Crème brûlée, 2 portions!  ");
///     // slug == "creme-brulee-2-portions"
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
    ///     Converts <paramref name="text" /> into a URL-safe slug: lower-case ASCII letters and digits, with words
    ///     separated by single hyphens.
    /// </summary>
    /// <remarks>
    ///     Accents are removed from letters (<c>é</c> becomes <c>e</c>). Every other character that is not an ASCII
    ///     letter or digit, including letters from non-Latin scripts, separates words.
    /// </remarks>
    /// <param name="text">The text to convert.</param>
    /// <returns>The slug, or an empty string when <paramref name="text" /> contains no ASCII letters or digits.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="text" /> is <see langword="null" />.</exception>
    public static string ToSlug(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        // Decomposition splits an accented letter into its base letter and a combining mark, which is then dropped.
        var decomposed = text.Normalize(NormalizationForm.FormD);
        var slug = new StringBuilder(decomposed.Length);
        var pendingSeparator = false;

        foreach (var character in decomposed)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(character) == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            if (char.IsAsciiLetterOrDigit(character))
            {
                if (pendingSeparator && slug.Length > 0)
                {
                    slug.Append('-');
                }

                slug.Append(char.ToLowerInvariant(character));
                pendingSeparator = false;
            }
            else
            {
                pendingSeparator = true;
            }
        }

        return slug.ToString();
    }
}
