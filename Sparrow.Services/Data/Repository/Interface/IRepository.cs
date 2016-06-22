using Sparrow.Services.Models;

namespace Sparrow.Services.Data.Repository.Interface
{
    public interface IRepository
    {
        object Get(int id);
        int Create(IModel model);
        void Update(object model);
        void Delete(object model);
    }
}
