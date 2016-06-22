namespace Sparrow.Services.File.Interface
{
    public interface IImageFileManager
    {
        void Create(string folderName, string base64File, int fileNum);
        void Update(string folderName, string base64File, int fileNum);
        void Delete(string filePath, int fileNum);
    }
}
