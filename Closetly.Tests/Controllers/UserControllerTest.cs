using System;
using System.Collections.Generic;
using Closetly.Controllers;
using Closetly.DTO;
using Closetly.Models;
using Closetly.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Closetly.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserService> _userServiceMock = null!;
        private UserController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
            _controller = new UserController(_userServiceMock.Object);

            // (opcional, mas útil se você algum dia usar HttpContext/StatusCodes via ControllerContext)
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        // -------------------------
        // CreateUser
        // -------------------------

        [Test]
        public void CreateUser_ShouldReturnOk_WhenUserIsCreated()
        {
            // Arrange
            var dto = new UserDTO
            {
                Id = Guid.NewGuid(),
                UserName = "Marco",
                Phone = "61999999999",
                Email = "marco@email.com"
            };

            _userServiceMock.Setup(s => s.CreateUser(dto)).Returns(dto);

            // Act
            var result = _controller.CreateUser(dto);

            // Assert
            var ok = result as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
            Assert.That(ok!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(ok.Value, Is.SameAs(dto));

            _userServiceMock.Verify(s => s.CreateUser(dto), Times.Once);
        }

        [Test]
        public void CreateUser_ShouldReturnBadRequestWithProblemDetails_WhenCreateFails()
        {
            // Arrange
            var dto = new UserDTO
            {
                Id = Guid.NewGuid(),
                UserName = "Marco",
                Phone = "61999999999",
                Email = "marco@email.com"
            };

            _userServiceMock.Setup(s => s.CreateUser(dto)).Returns((UserDTO?)null);

            // Act
            var result = _controller.CreateUser(dto);

            // Assert
            var bad = result as BadRequestObjectResult;
            Assert.That(bad, Is.Not.Null);
            Assert.That(bad!.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));

            var pd = bad.Value as ProblemDetails;
            Assert.That(pd, Is.Not.Null);
            Assert.That(pd!.Status, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(pd.Title, Is.EqualTo("Erro ao criar usuário"));
            Assert.That(pd.Detail, Is.EqualTo("Não foi possível criar o usuário."));

            _userServiceMock.Verify(s => s.CreateUser(dto), Times.Once);
        }

        // -------------------------
        // UpdateUser
        // -------------------------

        [Test]
        public void UpdateUser_ShouldReturnNoContent_WhenUpdateSucceeds()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new UpdateUserRequest { Name = "Novo Nome" };

            _userServiceMock.Setup(s => s.UpdateUser(id, request)).Returns("");

            // Act
            var result = _controller.UpdateUser(id, request);

            // Assert
            Assert.That(result, Is.TypeOf<NoContentResult>());
            var noContent = (NoContentResult)result;
            Assert.That(noContent.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));

            _userServiceMock.Verify(s => s.UpdateUser(id, request), Times.Once);
        }

        [Test]
        public void UpdateUser_ShouldReturnBadRequestWithProblemDetails_WhenServiceReturnsError()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new UpdateUserRequest { Email = "x@y.com" };
            var error = $"Usuário com a id {id} não encontrado.";

            _userServiceMock.Setup(s => s.UpdateUser(id, request)).Returns(error);

            // Act
            var result = _controller.UpdateUser(id, request);

            // Assert
            var bad = result as BadRequestObjectResult;
            Assert.That(bad, Is.Not.Null);
            Assert.That(bad!.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));

            var pd = bad.Value as ProblemDetails;
            Assert.That(pd, Is.Not.Null);
            Assert.That(pd!.Status, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(pd.Title, Is.EqualTo("Usuário não existe na base de dados"));
            Assert.That(pd.Detail, Is.EqualTo(error));

            _userServiceMock.Verify(s => s.UpdateUser(id, request), Times.Once);
        }

        // -------------------------
        // GetUserOrders
        // -------------------------

        [Test]
        public void GetUserOrders_ShouldReturnOk_WhenOrdersExist()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var orders = new List<UserOrders>
            {
                new UserOrders
                {
                    OrderId = Guid.NewGuid(),
                    UserId = userId,
                    UserName = "Marco",
                    OrderStatus = OrderStatus.CONCLUDED
                }
            };

            _userServiceMock.Setup(s => s.GetUserOrders(userId)).Returns(orders);

            // Act
            var result = _controller.GetUserOrders(userId);

            // Assert
            var ok = result as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
            Assert.That(ok!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(ok.Value, Is.SameAs(orders));

            _userServiceMock.Verify(s => s.GetUserOrders(userId), Times.Once);
        }

        [Test]
        public void GetUserOrders_ShouldReturnBadRequestWithProblemDetails_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userServiceMock.Setup(s => s.GetUserOrders(userId)).Returns((List<UserOrders>?)null);

            // Act
            var result = _controller.GetUserOrders(userId);

            // Assert
            var bad = result as BadRequestObjectResult;
            Assert.That(bad, Is.Not.Null);
            Assert.That(bad!.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));

            var pd = bad.Value as ProblemDetails;
            Assert.That(pd, Is.Not.Null);
            Assert.That(pd!.Status, Is.EqualTo(StatusCodes.Status400BadRequest));
            Assert.That(pd.Title, Is.EqualTo("Usuário não existe na base de dados"));
            Assert.That(pd.Detail, Is.EqualTo("Usuário não foi encontrado para o id informado"));

            _userServiceMock.Verify(s => s.GetUserOrders(userId), Times.Once);
        }

        // -------------------------
        // GetAllUsers
        // -------------------------

        [Test]
        public void GetAllUsers_ShouldReturnOk_WithUsersList()
        {
            // Arrange
            var users = new List<UserDTO>
            {
                new UserDTO { Id = Guid.NewGuid(), UserName = "A", Phone = "1", Email = "a@email.com" },
                new UserDTO { Id = Guid.NewGuid(), UserName = "B", Phone = "2", Email = "b@email.com" }
            };

            _userServiceMock.Setup(s => s.GetUsers()).Returns(users);

            // Act
            var result = _controller.GetAllUsers();

            // Assert
            var ok = result as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
            Assert.That(ok!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(ok.Value, Is.SameAs(users));

            _userServiceMock.Verify(s => s.GetUsers(), Times.Once);
        }
    }
}
