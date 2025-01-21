using Task = TaskManagement.Domain.Models.Task;

namespace TaskManagement.Domain.IRepositories
{
    public interface ITaskRepository
    {
        IEnumerable<Task> GetAll();
        Task GetById(int id);
        void Add(Task task);
        void Update(Task task);
        void Remove(int id);
    }
}
