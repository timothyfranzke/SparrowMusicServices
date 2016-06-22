using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Sparrow.Services.Utils.Interface
{
    public interface IFileManager
    {
        
        void Create(string filePath, string base64File, int fileNum);
        void Update(string filePath, string base64File, int fileNum);
        void Delete(int fileNum);
    }
}
