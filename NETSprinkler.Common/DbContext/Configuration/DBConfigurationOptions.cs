namespace NETSprinkler.Business.DbContext.Configuration;

public class DbConfigurationOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string ApplicationName { get; set; } = string.Empty;
}