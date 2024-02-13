namespace BLL.Services.Interfaces;

public interface IFileStorageService
{
    public Task<FileStream> GenerateProjectFiles(string code);
}