using TaskManagement.Domain.IRepositories;
using Task = TaskManagement.Domain.Models.Task;

namespace TaskManagement.Application.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public IEnumerable<Task> GetAllTasks()
        {
            return _taskRepository.GetAll();
        }

        public Task GetTaskById(int id)
        {
            return _taskRepository.GetById(id);
        }

        public void CreateTask(Task task)
        {
            task.CreationDate = DateTime.Now;
            _taskRepository.Add(task);
        }

        public void UpdateTask(Task task)
        {
            _taskRepository.Update(task);
        }

        public void DeleteTask(int id)
        {
            _taskRepository.Remove(id);
        }
    }
}
