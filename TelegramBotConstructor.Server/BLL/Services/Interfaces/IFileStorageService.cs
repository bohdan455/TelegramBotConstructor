namespace BLL.Services.Realizations;

public interface IFileStorageService
{
    void CopyDirectory(string sourceDir, string destinationDir, bool recursive);
    Task<FileStream> CreateArchiveFromFolder(string projectPath ,string zipFilePath);
    Task<string> BuildProject(string directory);
    Task ChangeProjectCode(string pathToProject, string code);
    Task WaitForFileCreating(string filePath);
}