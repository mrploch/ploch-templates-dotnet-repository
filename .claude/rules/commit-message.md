# Commit Message Guidelines

Create a well-structured git commit.
Commit message should follow the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) conventions:

```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

In the body, provide a more detailed description of the changes, in the footer, provide the GitHub issue or PR number using `Refs: #123`.

If the change is a breaking change, include `!` after the type / scope and `BREAKING CHANGE: ` in the body or footer, followed by a description of the breaking change.
