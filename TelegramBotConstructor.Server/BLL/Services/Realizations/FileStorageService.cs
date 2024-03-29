﻿using System.Diagnostics;
using System.IO.Compression;

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
    
    public Task<Stream> CreateArchiveFromFolder(string projectPath ,string zipFilePath)
    {
        var projectDebugPath = Path.Combine(projectPath, ArchiveConfigurations.SourcePath);
        
        var zipStream = new MemoryStream();

        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            AddFolderToZip(archive, projectDebugPath, string.Empty);
        }
        
        zipStream.Seek(0, SeekOrigin.Begin);
        return Task.FromResult<Stream>(zipStream);
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

    public Task RestoreProject(string pathToProject)
    {
        const string restoreProjectCommand = $"dotnet restore";
        return ExecuteCommand(restoreProjectCommand, pathToProject);
    }
    
    private static void AddFolderToZip(ZipArchive archive, string folderPath, string parentFolder)
    {
        foreach (string file in Directory.GetFiles(folderPath))
        {
            string entryName = Path.Combine(parentFolder, Path.GetFileName(file));
            archive.CreateEntryFromFile(file, entryName);
        }

        foreach (string subFolder in Directory.GetDirectories(folderPath))
        {
            string entryName = Path.Combine(parentFolder, Path.GetFileName(subFolder) + "/");
            archive.CreateEntry(entryName); 
            
            AddFolderToZip(archive, subFolder, entryName);
        }
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