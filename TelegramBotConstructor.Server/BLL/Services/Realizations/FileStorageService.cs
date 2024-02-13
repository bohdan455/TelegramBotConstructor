using System.Diagnostics;
using System.IO.Compression;
using BLL.Services.Interfaces;
using Configurations;

namespace BLL.Services.Realizations;

public class FileStorageService : IFileStorageService
{
    public async Task<FileStream> GenerateProjectFiles(string code)
    {
        var randomProjectName = $"Project{Guid.NewGuid()}";
        var workingDirectory = @"D:\projects";
        var projectPath = Path.Combine(workingDirectory, randomProjectName);

        await CreateProject(workingDirectory, randomProjectName);
        await ChangeProjectCode(Path.Combine(workingDirectory, randomProjectName), code);
        await BuildProject(Path.Combine(workingDirectory, randomProjectName));
        
        var archivePath = Path.Combine(projectPath, $"{randomProjectName}.zip");
        return await CreateArchiveFromFolder(projectPath, archivePath);
    }
    
    private async Task<FileStream> CreateArchiveFromFolder(string projectPath ,string zipFilePath)
    {
        var projectDebugPath = Path.Combine(projectPath, ArchiveConfigurations.SourcePath);
        ZipFile.CreateFromDirectory(projectDebugPath, zipFilePath);
        await WaitForFile(zipFilePath);
        return File.OpenRead(zipFilePath);
    }
    
    private static Task<string> CreateProject(string directory, string fileName)
    {
        var createAppCommand = $"dotnet new console -n {fileName}";
        return ExecuteCommand(createAppCommand, directory);
    }

    private static Task<string> BuildProject(string directory)
    {
        const string buildProjectCommand = $"dotnet build";
        return ExecuteCommand(buildProjectCommand, directory);
    }
    
    private static async Task<string> ExecuteCommand(string command, string directory)
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
    
    private static Task ChangeProjectCode(string pathToProject, string code)
    {
        var textToWrite = code;
        var pathToFile = Path.Combine(pathToProject, "Program.cs");
        return File.WriteAllTextAsync(pathToFile, textToWrite);   
    }

    private static async Task WaitForFile(string filePath)
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
}