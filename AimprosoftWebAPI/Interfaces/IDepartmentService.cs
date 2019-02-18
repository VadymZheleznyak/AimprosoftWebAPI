using System.Collections.Generic;
using System.Threading.Tasks;

namespace AimprosoftWebAPI.Interfaces
{
    public interface IDepartmentService<T>
    {
        IEnumerable<T> GetAll();
        Task<T> Get(int id);
        Task<bool> Put(int id, T subj);
        Task Post(T subj);
        Task Delete(T subj);
        List<T> GetByKey(string key, int pageNum, int pageSize);
        bool CheckExisting(T subj);
        int GetCount();
        List<T> GetForPaging(int pageNum, int pageSize);
    }
}
