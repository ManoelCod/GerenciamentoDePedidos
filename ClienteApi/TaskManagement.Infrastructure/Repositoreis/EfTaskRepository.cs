using TaskManagement.Infrastructure.Data;
using Task = TaskManagement.Domain.Models.Task
    ;
using TaskManagement.Domain.IRepositories;

namespace TaskManagement.Infrastructure.Repositories
{
    public class EfTaskRepository : ITaskRepository
    {
        private readonly TaskDbContext _context;

        public EfTaskRepository(TaskDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Task> GetAll()
        {
            return _context.Tasks.ToList();
        }

        public Task GetById(int id)
        {
            return _context.Tasks.Find(id);
        }

        public void Add(Task task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        public void Update(Task task)
        {
            _context.Tasks.Update(task);
            _context.SaveChanges();
        }

        public void Remove(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
        }
    }
}
