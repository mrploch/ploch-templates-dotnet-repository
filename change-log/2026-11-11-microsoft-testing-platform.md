### Changed

- Tests run on Microsoft.Testing.Platform, which xUnit 4 requires on the .NET 10 SDK; `global.json` selects it.
  Pass a solution with `dotnet test --solution <file>`, not as a bare argument.
- Coverage comes from coverlet.MTP and TRX reports from Microsoft.Testing.Extensions.TrxReport. CI writes both to
  `TestResults/`; `Microsoft.NET.Test.Sdk`, `xunit.runner.visualstudio` and `coverlet.msbuild` are no longer used.
