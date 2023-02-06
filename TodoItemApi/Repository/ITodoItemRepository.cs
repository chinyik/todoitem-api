using TodoItemApi.Models;

namespace TodoItemApi.Repository
{
    public interface ITodoItemRepository
    {
        Task<List<TodoItem>?> GetAllTodoItemsAsync(string nameFilter,
            string descriptionFilter,
            DateTime? dueDateFilter,
            string statusFilter,
            string sortBy,
            bool isDescending);
        Task<TodoItem?> GetTodoItemByIdAsync(int id);
        Task<int> UpdateTodoItemByIdAsync(int id, TodoItem item);
        Task<int> AddTodoItemAsync(TodoItem item);
        Task<int> DeleteTodoItemAsync(int id);
    }
}
