using Microsoft.EntityFrameworkCore;
using Moq;
using TodoItemApi.Data;
using TodoItemApi.Models;
using TodoItemApi.Repository;

namespace TodoItemApiTest
{
    public class TodoItemRepositoryTest
    {
        private List<TodoItem> _todoItems;

        [SetUp]
        public void SetUp()
        {
            _todoItems = new()
            {
                new TodoItem
                {
                    TodoItemId = 1,
                    Name = "TestName1",
                    Description = "TestDescription1",
                    DueDate = new DateTime(2023, 2, 6),
                    Status = "TestStatus1"
                },
                new TodoItem
                {
                    TodoItemId = 2,
                    Name = "TestName2",
                    Description = "TestDescription2",
                    DueDate = new DateTime(2023, 2, 6),
                    Status = "TestStatus2"
                },
                new TodoItem
                {
                    TodoItemId = 5,
                    Name = "TestName5",
                    Description = "TestDescription5",
                    DueDate = new DateTime(2023, 2, 6),
                    Status = "TestStatus5"
                }
            };
        }

        [Test]
        public async Task TestGetAllTodoItemByIdAsync_WithoutFilters_ShouldReturnAllItems()
        {
            // Arrange
            DbContextOptions<TodoItemContext> options = new DbContextOptionsBuilder<TodoItemContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (TodoItemContext context = new(options))
            {
                context.TodoItems.AddRange(_todoItems);
                context.SaveChanges();
            }

            using (TodoItemContext context = new(options))
            {
                ITodoItemRepository repository = new TodoItemRepository(context);

                //Act
                List<TodoItem> resultItem = await repository.GetAllTodoItemsAsync(
                    string.Empty,
                    string.Empty,
                    null,
                    string.Empty,
                    string.Empty,
                    true);

                //Assert
                Assert.That(resultItem, Is.Not.Null);
                Assert.That(resultItem.Count, Is.EqualTo(_todoItems.Count));
            }
        }

        [Test]
        public async Task TestGetAllTodoItemByIdAsync_WithFilters_ShouldReturnMatchingItems()
        {
            // Arrange
            DbContextOptions<TodoItemContext> options = new DbContextOptionsBuilder<TodoItemContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (TodoItemContext context = new(options))
            {
                context.TodoItems.AddRange(_todoItems);
                context.SaveChanges();
            }

            using (TodoItemContext context = new(options))
            {
                ITodoItemRepository repository = new TodoItemRepository(context);

                //Act
                List<TodoItem> resultItem = await repository.GetAllTodoItemsAsync(
                    "test",
                    string.Empty,
                    null,
                    string.Empty,
                    string.Empty,
                    true);

                //Assert
                Assert.That(resultItem, Is.Not.Null);
                Assert.That(resultItem.Count, Is.EqualTo(_todoItems.Where(x => x.Name.ToLower().Contains("test")).Count()));
            }
        }

        [Test]
        public async Task TestGetTodoItemByIdAsync_IfTableExists_ShouldReturnItem()
        {
            // Arrange
            DbContextOptions<TodoItemContext> options = new DbContextOptionsBuilder<TodoItemContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (TodoItemContext context = new(options))
            {
                context.TodoItems.AddRange(_todoItems);
                context.SaveChanges();
            }

            using (TodoItemContext context = new(options))
            {
                ITodoItemRepository repository = new TodoItemRepository(context);

                //Act
                TodoItem resultItem = await repository.GetTodoItemByIdAsync(5);

                //Assert
                Assert.That(resultItem, Is.Not.Null);
                Assert.Multiple(() =>
                {
                    Assert.That(resultItem.TodoItemId, Is.EqualTo(5));
                    Assert.That(_todoItems.Any(x => x.TodoItemId == resultItem.TodoItemId), Is.True);
                });
            }
        }

        [Test]
        public async Task TestGetTodoItemByIdAsync_IfTableNotExists_ShouldReturnNull()
        {
            // Arrange
            DbContextOptions<TodoItemContext> options = new DbContextOptionsBuilder<TodoItemContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (TodoItemContext context = new(options))
            {
                ITodoItemRepository repository = new TodoItemRepository(context);

                //Act
                TodoItem resultItem = await repository.GetTodoItemByIdAsync(5);

                //Assert
                Assert.That(resultItem, Is.Null);
            }
        }

        [Test]
        public async Task TestUpdateTodoItemByIdAsync_IfItemExists_ShouldReturnItem()
        {
            // Arrange
            DbContextOptions<TodoItemContext> options = new DbContextOptionsBuilder<TodoItemContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (TodoItemContext context = new(options))
            {
                context.TodoItems.AddRange(_todoItems);
                context.SaveChanges();
            }

            using (TodoItemContext context = new(options))
            {
                ITodoItemRepository repository = new TodoItemRepository(context);

                TodoItem item = new()
                {
                    TodoItemId = 5,
                    Name = "TestUpdateName5",
                    Description = "TestUpdateDescription5",
                    DueDate = DateTime.Now,
                    Status = "TestUpdateStatus5"
                };

                //Act
                int result = await repository.UpdateTodoItemByIdAsync(5, item);

                //Assert
                Assert.That(result, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task TestUpdateTodoItemByIdAsync_IfItemNotExists_ShouldReturnNull()
        {
            // Arrange
            DbContextOptions<TodoItemContext> options = new DbContextOptionsBuilder<TodoItemContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (TodoItemContext context = new(options))
            {
                context.TodoItems.AddRange(_todoItems);
                context.SaveChanges();
            }

            using (TodoItemContext context = new(options))
            {
                ITodoItemRepository repository = new TodoItemRepository(context);

                TodoItem item = new()
                {
                    TodoItemId = 4,
                    Name = "TestUpdateName4",
                    Description = "TestUpdateDescription4",
                    DueDate = DateTime.Now,
                    Status = "TestUpdateStatus4"
                };

                //Act
                int result = await repository.UpdateTodoItemByIdAsync(4, item);

                //Assert
                Assert.That(result, Is.EqualTo(0));
            }
        }

        [Test]
        public async Task TestAddTodoItemByIdAsync_IfTableExists_ShouldReturnItem()
        {
            // Arrange
            DbContextOptions<TodoItemContext> options = new DbContextOptionsBuilder<TodoItemContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (TodoItemContext context = new(options))
            {
                context.TodoItems.AddRange(_todoItems);
                context.SaveChanges();
            }

            using (TodoItemContext context = new(options))
            {
                ITodoItemRepository repository = new TodoItemRepository(context);

                TodoItem item = new()
                {
                    TodoItemId = 4,
                    Name = "TestNewName4",
                    Description = "TestNewDescription4",
                    DueDate = DateTime.Now,
                    Status = "TestNewStatus4"
                };

                //Act
                int result = await repository.AddTodoItemAsync(item);

                //Assert
                Assert.That(result, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task TestDeleteTodoItemByIdAsync_IfItemExists_ShouldReturnItem()
        {
            // Arrange
            DbContextOptions<TodoItemContext> options = new DbContextOptionsBuilder<TodoItemContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (TodoItemContext context = new(options))
            {
                context.TodoItems.AddRange(_todoItems);
                context.SaveChanges();
            }

            using (TodoItemContext context = new(options))
            {
                ITodoItemRepository repository = new TodoItemRepository(context);

                //Act
                int result = await repository.DeleteTodoItemAsync(5);

                //Assert
                Assert.That(result, Is.EqualTo(1));
            }
        }

        [Test]
        public async Task TestDeleteTodoItemByIdAsync_IfItemNotExists_ShouldReturnNull()
        {
            // Arrange
            DbContextOptions<TodoItemContext> options = new DbContextOptionsBuilder<TodoItemContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using (TodoItemContext context = new(options))
            {
                context.TodoItems.AddRange(_todoItems);
                context.SaveChanges();
            }

            using (TodoItemContext context = new(options))
            {
                ITodoItemRepository repository = new TodoItemRepository(context);

                //Act
                int result = await repository.DeleteTodoItemAsync(4);

                //Assert
                Assert.That(result, Is.EqualTo(0));
            }
        }
    }
}
