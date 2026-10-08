using Ploch.Data.EFCore.SqLite;
using Ploch.MyApp.Data;

namespace Ploch.MyApp.Data.SQLite;

public class MyAppDbContextFactory()
    : SqLiteDbContextFactory<MyAppDbContext, MyAppDbContextFactory>(options => new(options));
