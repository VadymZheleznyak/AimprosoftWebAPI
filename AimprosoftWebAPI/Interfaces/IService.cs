using System.Collections.Generic;
using System.Threading.Tasks;

namespace AimprosoftWebAPI.Interfaces
{
    public interface IService<T>
    {
        IEnumerable<T> GetAll();
        Task<T> Get(int id);
        Task<bool> Put(int id, T subj);
        Task Post(T subj);
        Task Delete(T subj);
        List<T> GetByKey(string key);
        List<T> GetByRelationId(int id);
        bool CheckExisting(T subj);
        int GetCount();
    }
}
