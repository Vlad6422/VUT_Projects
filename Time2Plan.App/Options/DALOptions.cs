namespace Time2Plan.App.Options;

public record DALOptions
{
    public SqliteOptions Sqlite = new();
}

public record SqliteOptions
{
    public bool Enabled { get; init; } = true;

    public string DatabaseName { get; init; } = "time2plan.db";
    /// <summary>
    /// Deletes database before application startup
    /// </summary>
    public bool RecreateDatabaseEachTime { get; init; } = false;

    /// <summary>
    /// Seeds DemoData from DbContext on database creation.
    /// </summary>
    public bool SeedDemoData { get; init; } = false;
}
