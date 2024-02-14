using System.Diagnostics;
using System.IO.Compression;
using BLL.Services.Interfaces;
using Configurations;

namespace BLL.Services.Realizations;

public class FileStorageService : IFileStorageService
{
    public void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
    {
        var dir = new DirectoryInfo(sourceDir);
    
        if (!dir.Exists)
            throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");
    
        var dirs = dir.GetDirectories();
    
        Directory.CreateDirectory(destinationDir);
    
        foreach (var file in dir.GetFiles())
        {
            var targetFilePath = Path.Combine(destinationDir, file.Name);
            file.CopyTo(targetFilePath);
        }


        if (!recursive) return;
    
        foreach (var subDir in dirs)
        {
            var newDestinationDir = Path.Combine(destinationDir, subDir.Name);
            CopyDirectory(subDir.FullName, newDestinationDir, true);
        }
    }
    
    public async Task<FileStream> CreateArchiveFromFolder(string projectPath ,string zipFilePath)
    {
        var projectDebugPath = Path.Combine(projectPath, ArchiveConfigurations.SourcePath);
        ZipFile.CreateFromDirectory(projectDebugPath, zipFilePath);
        await WaitForFileCreating(zipFilePath);
        return File.OpenRead(zipFilePath);
    }

    public Task<string> BuildProject(string directory)
    {
        const string buildProjectCommand = $"dotnet build";
        return ExecuteCommand(buildProjectCommand, directory);
    }
    
    public Task ChangeProjectCode(string pathToProject, string code)
    {
        var pathToFile = Path.Combine(pathToProject, "Program.cs");
        return File.WriteAllTextAsync(pathToFile, code);   
    }

    public async Task WaitForFileCreating(string filePath)
    {
        const int maxAttempts = ArchiveConfigurations.MaxCreatingAttempts;
        const int delayMilliseconds = ArchiveConfigurations.CreatingCheckDelayMilliseconds;

        for (var i = 0; i < maxAttempts; i++)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    await using var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
                    return;
                }
                catch (IOException)
                {
                }
            }
        
            await Task.Delay(delayMilliseconds);
        }

        throw new TimeoutException("File access timeout exceeded.");
    }
    
    private async Task<string> ExecuteCommand(string command, string directory)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = "/c" + command, 
            WorkingDirectory = directory, 
            RedirectStandardOutput = true, 
            CreateNoWindow = true 
        };

        var process = new Process();
        process.StartInfo = processStartInfo;
        process.Start();

        var output = await process.StandardOutput.ReadToEndAsync();
    
        await process.WaitForExitAsync();
        return output;
    }
}