using Microsoft.EntityFrameworkCore;
using TodoItemApi.Data;
using TodoItemApi.Models;

namespace TodoItemApi.Repository
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private TodoItemContext _context;

        public TodoItemRepository(TodoItemContext context)
        {
            _context = context;
        }

        public async Task<List<TodoItem>?> GetAllTodoItemsAsync(
            string name = "",
            string description = "",
            DateTime? dueDate = null,
            string status = "",
            string sortBy = "",
            bool isDescending = false)
        {
            if (!IsTableExists())
            {
                return null;
            }

            IQueryable<TodoItem> query = _context.TodoItems.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.ToLower().Contains(name));
            }

            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(x => x.Description.ToLower().Contains(description));
            }

            if (dueDate != null)
            {
                query = query.Where(x => x.DueDate.Day == dueDate.Value.Day);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(x => x.Status.ToLower().Contains(status));
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                SortTodoItems(query, sortBy, isDescending);
            }

            if (isDescending == true)
            {
                query = query.OrderByDescending(x => x.TodoItemId);
            }
            else
            {
                query = query.OrderBy(x => x.TodoItemId);
            }

            return await query.ToListAsync();
        }

        public async Task<TodoItem?> GetTodoItemByIdAsync(int id)
        {
            return (IsTableExists())
                ? await _context.TodoItems.FindAsync(id)
                : null;
        }

        public async Task<int> UpdateTodoItemByIdAsync(int id, TodoItem item)
        {
            if (!TodoItemExists(id))
            {
                return 0;
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task<int> AddTodoItemAsync(TodoItem item)
        {
            if (!IsTableExists())
            {
                return 0;
            }

            item.Status = "todo";
            try
            {
                _context.TodoItems.Add(item);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteTodoItemAsync(int id)
        {
            if (!TodoItemExists(id))
            {
                return 0;
            }

            try
            {
                var todoItem = await _context.TodoItems.FindAsync(id);
                if (todoItem == null)
                {
                    return 0;
                }

                _context.TodoItems.Remove(todoItem);
                return await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SortTodoItems(IQueryable<TodoItem> query, string sortOrder, bool? isDescending)
        {
            switch (sortOrder.ToLower())
            {
                case "name":
                    if (isDescending != null && isDescending.Value == true)
                    {
                        query = query.OrderByDescending(x => x.Name);
                    }
                    else
                    {
                        query = query.OrderBy(x => x.Name);
                    }
                    break;
                case "description":
                    if (isDescending != null && isDescending.Value == true)
                    {
                        query = query.OrderByDescending(x => x.Description);
                    }
                    else
                    {
                        query = query.OrderBy(x => x.Description);
                    }
                    break;
                case "status":
                    if (isDescending != null && isDescending.Value == true)
                    {
                        query = query.OrderByDescending(x => x.Status);
                    }
                    else
                    {
                        query = query.OrderBy(x => x.Status);
                    }
                    break;
                default:
                    if (isDescending != null && isDescending.Value == true)
                    {
                        query = query.OrderByDescending(x => x.TodoItemId);
                    }
                    else
                    {
                        query = query.OrderBy(x => x.TodoItemId);
                    }
                    break;
            }
        }

        private bool IsTableExists()
        {
            return _context.TodoItems != null;
        }

        private bool TodoItemExists(int id)
        {
            return (_context.TodoItems?.Any(e => e.TodoItemId == id)).GetValueOrDefault();
        }
    }
}
