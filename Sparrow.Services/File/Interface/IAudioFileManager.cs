using System.Web;

namespace Sparrow.Services.File.Interface
{
    public interface IAudioFileManager
    {
        void Create(string filePath, HttpPostedFileBase file, int fileNum, string fileType);
        void Create(string filePath, byte[] file, int fileNum, string fileType);
        void Update(string filePath, HttpPostedFileBase file, int fileNum);
        void Delete(string filePath, int fieNum);
    }
}
