namespace Time2Plan.App.Options;

public class DALSettings
{
    public string ConnectionString { get; set; }
    public bool SeedDemoData { get; set; }
}

public record DALOptions
{
    public LocalDbOptions LocalDb { get; init; }
}

public record LocalDbOptions
{
    //public bool Enabled { get; init; }
    public string? ConnectionString { get; init; }
    public bool SeedDemoData { get; init; }
}

//public record SqliteOptions
//{
//    public bool Enabled { get; init; }

//    public string DatabaseName { get; init; } = null!;
//    /// <summary>
//    /// Deletes database before application startup
//    /// </summary>
//    public bool RecreateDatabaseEachTime { get; init; } = false;

//    /// <summary>
//    /// Seeds DemoData from DbContext on database creation.
//    /// </summary>
//    public bool SeedDemoData { get; init; } = false;
//}
