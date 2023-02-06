using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoItemApi.Controllers;
using TodoItemApi.Models;
using TodoItemApi.Repository;

namespace TodoItemApiTest
{
    public class TodoItemControllerTests
    {
        private TodoItemsController _controller;

        private Mock<ITodoItemRepository> _mockRepository;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<ITodoItemRepository>();
            _controller = new TodoItemsController(_mockRepository.Object);
        }

        [Test]
        public async Task TestGetTodoItems_WhenSuccess_ShouldReturnOk()
        {
            // Arrange
            List<TodoItem> todoItems = new()
            {
                new TodoItem
                {
                    TodoItemId = 1,
                    Name = "TestName1",
                    Description = "TestDescription1",
                    DueDate = DateTime.Now,
                    Status = "TestStatus1"
                },
                new TodoItem
                {
                    TodoItemId = 2,
                    Name = "TestName2",
                    Description = "TestDescription2",
                    DueDate = DateTime.Now,
                    Status = "TestStatus2"
                },
                new TodoItem
                {
                    TodoItemId = 3,
                    Name = "TestName3",
                    Description = "TestDescription3",
                    DueDate = DateTime.Now,
                    Status = "TestStatus3"
                }
            };

            _mockRepository.Setup(x => x.GetAllTodoItemsAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
                .ReturnsAsync(todoItems);

            // Act
            ActionResult<IEnumerable<TodoItem>> response = await _controller.GetTodoItems(
                                                            It.IsAny<string>(),
                                                            It.IsAny<string>(),
                                                            It.IsAny<DateTime>(),
                                                            It.IsAny<string>(),
                                                            It.IsAny<string>(),
                                                            It.IsAny<bool>());

            // Assert
            _mockRepository.Verify(x => x.GetAllTodoItemsAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>()), Times.Once);
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Result, Is.Not.Null);
            Assert.That(response.Result, Is.TypeOf<OkObjectResult>());
            OkObjectResult responseResult = response.Result as OkObjectResult;
            IEnumerable<TodoItem> responseItem = responseResult.Value as IEnumerable<TodoItem>;
            Assert.That(responseItem, Is.Not.Null);
            Assert.That(responseItem.Count, Is.EqualTo(todoItems.Count));
        }

        [Test]
        public async Task TestGetTodoItems_WhenNull_ShouldReturnNotFound()
        {
            // Arrange
            _mockRepository.Setup(x => x.GetAllTodoItemsAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>()))
                .ReturnsAsync((List<TodoItem>)null);

            // Act
            ActionResult<IEnumerable<TodoItem>> response = await _controller.GetTodoItems(
                                                            It.IsAny<string>(),
                                                            It.IsAny<string>(),
                                                            It.IsAny<DateTime>(),
                                                            It.IsAny<string>(),
                                                            It.IsAny<string>(),
                                                            It.IsAny<bool>());

            // Assert
            _mockRepository.Verify(x => x.GetAllTodoItemsAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>()), Times.Once);
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Result, Is.Not.Null);
            Assert.That(response.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task TestGetTodoItemById_WhenOk_ShouldReturnItem()
        {
            // Arrange
            TodoItem item = new()
            {
                TodoItemId = 5,
                Name = "TestName5",
                Description = "TestDescription5",
                DueDate = DateTime.Now,
                Status = "TestStatus5"
            };
            _mockRepository.Setup(x => x.GetTodoItemByIdAsync(5)).ReturnsAsync(item);

            // Act
            ActionResult<TodoItem> response = await _controller.GetTodoItemById(5);

            // Assert
            _mockRepository.Verify(x => x.GetTodoItemByIdAsync(5), Times.Once);
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Result, Is.Not.Null);
            Assert.That(response.Result, Is.TypeOf<OkObjectResult>());
            OkObjectResult responseResult = response.Result as OkObjectResult;
            TodoItem responseItem = responseResult.Value as TodoItem;
            Assert.That(responseItem, Is.Not.Null);
            Assert.That(responseItem.TodoItemId, Is.EqualTo(item.TodoItemId));
        }

        [Test]
        public async Task TestGetTodoItemById_WhenNull_ShouldReturnNotFound()
        {
            // Arrange
            _mockRepository.Setup(x => x.GetTodoItemByIdAsync(5)).ReturnsAsync((TodoItem)null);

            // Act
            ActionResult<TodoItem> response = await _controller.GetTodoItemById(5);

            // Assert
            _mockRepository.Verify(x => x.GetTodoItemByIdAsync(5), Times.Once);
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Result, Is.Not.Null);
            Assert.That(response.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task TestUpdateTodoItemById_WhenSuccess_ShouldReturnNoContent()
        {
            // Arrange
            TodoItem item = new()
            {
                TodoItemId = 5,
                Name = "TestName5",
                Description = "TestDescription5",
                DueDate = DateTime.Now,
                Status = "TestStatus5"
            };
            _mockRepository.Setup(x => x.UpdateTodoItemByIdAsync(It.IsAny<int>(), It.IsAny<TodoItem>())).ReturnsAsync(1);

            // Act
            IActionResult response = await _controller.UpdateTodoItemById(5, item);

            // Assert
            _mockRepository.Verify(x => x.UpdateTodoItemByIdAsync(5, item), Times.Once);
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task TestUpdateTodoItemById_WhenFail_ShouldReturnNotFound()
        {
            // Arrange
            TodoItem item = new()
            {
                TodoItemId = 5,
                Name = "TestName5",
                Description = "TestDescription5",
                DueDate = DateTime.Now,
                Status = "TestStatus5"
            };
            _mockRepository.Setup(x => x.UpdateTodoItemByIdAsync(It.IsAny<int>(), It.IsAny<TodoItem>())).ReturnsAsync(0);

            // Act
            IActionResult response = await _controller.UpdateTodoItemById(5, item);

            // Assert
            _mockRepository.Verify(x => x.UpdateTodoItemByIdAsync(5, item), Times.Once);
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task TestUpdateTodoItemById_WhenIdsNotMatch_ShouldReturnBadRequest()
        {
            // Arrange
            TodoItem item = new()
            {
                TodoItemId = 5,
                Name = "TestName5",
                Description = "TestDescription5",
                DueDate = DateTime.Now,
                Status = "TestStatus5"
            };
            _mockRepository.Setup(x => x.UpdateTodoItemByIdAsync(It.IsAny<int>(), It.IsAny<TodoItem>())).ReturnsAsync(1);

            // Act
            IActionResult response = await _controller.UpdateTodoItemById(3, item);

            // Assert
            _mockRepository.Verify(x => x.UpdateTodoItemByIdAsync(3, item), Times.Never);
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        public async Task TestAddTodoItem_WhenSuccess_ShouldReturnCreatedAtAction()
        {
            // Arrange
            TodoItem item = new()
            {
                TodoItemId = 5,
                Name = "TestName5",
                Description = "TestDescription5",
                DueDate = DateTime.Now,
                Status = "TestStatus5"
            };
            _mockRepository.Setup(x => x.AddTodoItemAsync(It.IsAny<TodoItem>())).ReturnsAsync(1);

            // Act
            ActionResult<TodoItem> response = await _controller.AddTodoItem(item);

            // Assert
            _mockRepository.Verify(x => x.AddTodoItemAsync(item), Times.Once);
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Result, Is.Not.Null);
            Assert.That(response.Result, Is.TypeOf<CreatedAtActionResult>());
            CreatedAtActionResult responseResult = response.Result as CreatedAtActionResult;
            TodoItem responseItem = responseResult.Value as TodoItem;
            Assert.That(responseItem, Is.Not.Null);
            Assert.That(responseItem.TodoItemId, Is.EqualTo(item.TodoItemId));
        }

        [Test]
        public async Task TestAddTodoItem_WhenFail_ShouldReturnNotFound()
        {
            // Arrange
            TodoItem item = new()
            {
                TodoItemId = 5,
                Name = "TestName5",
                Description = "TestDescription5",
                DueDate = DateTime.Now,
                Status = "TestStatus5"
            };
            _mockRepository.Setup(x => x.AddTodoItemAsync(It.IsAny<TodoItem>())).ReturnsAsync(0);

            // Act
            ActionResult<TodoItem> response = await _controller.AddTodoItem(item);

            // Assert
            _mockRepository.Verify(x => x.AddTodoItemAsync(item), Times.Once);
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Result, Is.Not.Null);
            Assert.That(response.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task TestDeleteTodoItem_WhenSuccess_ShouldReturnNoContent()
        {
            // Arrange
            _mockRepository.Setup(x => x.DeleteTodoItemAsync(It.IsAny<int>())).ReturnsAsync(1);

            // Act
            IActionResult response = await _controller.DeleteTodoItem(5);

            // Assert
            _mockRepository.Verify(x => x.DeleteTodoItemAsync(5), Times.Once);
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task TestDeleteTodoItem_WhenFail_ShouldReturnNotFound()
        {
            // Arrange
            _mockRepository.Setup(x => x.AddTodoItemAsync(It.IsAny<TodoItem>())).ReturnsAsync(0);

            // Act
            IActionResult response = await _controller.DeleteTodoItem(5);

            // Assert
            _mockRepository.Verify(x => x.DeleteTodoItemAsync(5), Times.Once);
            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.TypeOf<NotFoundResult>());
        }
    }
}