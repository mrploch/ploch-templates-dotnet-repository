# Change log entries

Every pull request with a user-visible change adds one Markdown file to this directory, named
`<yyyy-MM-dd>-<short-description>.md`, for example `2026-10-09-add-slug-options.md`:

```markdown
### Added

- `TextNormalizer.ToSlug` accepts a custom separator.
```

Use the headings `Added`, `Changed`, `Fixed`, `Removed` or `Security`.

The Release workflow concatenates the entries into the GitHub release notes, then moves them to
`change-log/archive/<version>/` in its bookkeeping pull request, so each entry is published exactly once.
