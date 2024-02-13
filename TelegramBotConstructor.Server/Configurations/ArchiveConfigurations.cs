namespace Configurations;

public static class ArchiveConfigurations
{
    public const int CreatingCheckDelayMilliseconds = 500;
    
    public const int MaxCreatingAttempts = 10;

    public static readonly string SourcePath = Path.Combine("bin", "Debug", "net8.0");
}