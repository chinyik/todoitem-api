using Microsoft.AspNetCore.Mvc;
using TodoItemApi.Models;
using TodoItemApi.Repository;

namespace TodoItemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemRepository _repository;

        public TodoItemsController(ITodoItemRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get all TodoItems, supply additional parameters to enable filtering or sorting,
        /// </summary>
        /// <param name="nameFilter">Filter TodoItems by name, it is optional.</param>
        /// <param name="descriptionFilter">Filter TodoItems by description, it is optional.</param>
        /// <param name="dueDateFilter">Filter TodoItems by due date, it is optional.</param>
        /// <param name="statusFilter">Filter TodoItems by status, it is optional.</param>
        /// <param name="sortBy">Sort TodoItems by column name</param>
        /// <param name="isDescending">Sort TodoItems by descending order if true, default is <c>false</c></param>
        /// <returns></returns>
        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems(
            string? nameFilter,
            string? descriptionFilter,
            DateTime? dueDateFilter,
            string? statusFilter,
            string? sortBy,
            bool? isDescending
            )
        {
            if (!isDescending.HasValue)
            {
                isDescending = false;
            }

            List<TodoItem>? todoItems = await _repository.GetAllTodoItemsAsync(nameFilter, descriptionFilter, dueDateFilter, statusFilter, 
                sortBy, isDescending.Value);

            return (todoItems == null)
                ? NotFound()
                : Ok(todoItems);
        }

        /// <summary>
        /// Get 1 TodoItem by matching id.
        /// </summary>
        /// <param name="id">Id of TodoItem to be fetched</param>
        /// <returns>Fetched TodoItem</returns>
        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItemById(int id)
        {
            var todoItem = await _repository.GetTodoItemByIdAsync(id);

            return (todoItem == null)
                ? NotFound()
                : Ok(todoItem);
        }

        /// <summary>
        /// Update 1 TodoItem by matching id.
        /// </summary>
        /// <param name="id">Id of TodoItem to be updated</param>
        /// <param name="todoItem">TodoItem details to be updated</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/TodoItems/{id}
        ///     {
        ///        "todoItemId": {id},
        ///        "name": "Item #1",
        ///        "description": "Description #1",
        ///        "dueDate": "2023-02-06",
        ///        "status": "todo"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItemById(int id, TodoItem todoItem)
        {
            if (id != todoItem.TodoItemId)
            {
                return BadRequest();
            }

            int rowsAffected = await _repository.UpdateTodoItemByIdAsync(id, todoItem);

            return (rowsAffected > 0)
                ? NoContent()
                : NotFound();
        }

        /// <summary>
        /// Add 1 TodoItem.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/TodoItems
        ///     {
        ///        "name": "Item #1",
        ///        "description": "Description #1",
        ///        "dueDate": "2023-02-06",
        ///        "status": "todo"
        ///     }
        ///
        /// </remarks>
        /// <returns>Created TodoItem</returns>
        // POST: api/TodoItems
        [HttpPost]
        public async Task<ActionResult<TodoItem>> AddTodoItem(TodoItem todoItem)
        {
            int rowsAffected = await _repository.AddTodoItemAsync(todoItem);

            return (rowsAffected > 0)
                ? CreatedAtAction("GetTodoItemById", new { id = todoItem.TodoItemId }, todoItem)
                : NotFound();
        }

        /// <summary>
        /// Delete 1 TodoItem by matching id.
        /// </summary>
        /// <param name="id">Id of TodoItem to be deleted</param>
        /// <returns></returns>
        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            int rowsAffected = await _repository.DeleteTodoItemAsync(id);

            return (rowsAffected > 0) 
                ? NoContent() 
                : NotFound();
        }
    }
}
