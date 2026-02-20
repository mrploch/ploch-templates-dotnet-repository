---
name: commit
description: Create a git commit following the project's commit conventions. Use when the user asks to commit changes, or after completing a task that should be committed.
allowed-tools: Bash(git:*), Glob, Read
---

# Git Commit

Create a well-structured git commit. If necessary, create multiple commits to logically separate changes. Follow the project's commit message conventions, which are typically based on [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/).
Commit message should follow the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) conventions:

```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

In the body, provide a more detailed description of the changes, in the footer, provide the GitHub issue or PR number.

## Process

1. **Check for uncommitted changes**:

   ```bash
   git status
   git diff
   git diff --staged
   ```

2. **Use conventional commits message**

See the commit message guidelines in `.claude/rules/commit-message.md` and [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) for detailed instructions on formatting the commit message.

4. Stage appropriate files with `git add`

## Important Rules

- **Never commit secrets** (.env, credentials, keys, etc.)
- **Always verify changes** before committing
- **Use a HEREDOC** for multi-line messages:

```bash
git commit -m "$(cat <<'EOF'
Your commit message here
EOF
)"
```
