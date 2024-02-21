namespace BLL.Services.Realizations;

public interface IFileStorageService
{
    void CopyDirectory(string sourceDir, string destinationDir, bool recursive);
    Task<Stream> CreateArchiveFromFolder(string projectPath, string zipFilePath);
    Task<string> BuildProject(string directory);
    Task ChangeProjectCode(string pathToProject, string code);
    
    Task RestoreProject(string pathToProject);
}