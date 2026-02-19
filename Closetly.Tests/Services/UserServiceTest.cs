using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Closetly.DTO;
using Closetly.Models;
using Closetly.Repository;
using Closetly.Services;
using Moq;


[TestFixture]
public class UserServiceTest
{
    private Mock<IUserRepository> _userRepositoryMock;
    private UserService _userService;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    // -------------------------
    // GetUsers
    // -------------------------

    [Test]
    public void GetUsers_ShouldMapTbUserToUserDTO()
    {
        // Arrange
        var repoUsers = new List<TbUser>
        {
            new TbUser
            {
                UserId = Guid.NewGuid(),
                UserName = "A",
                Phone = "1",
                Email = "a@email.com"
            },
            new TbUser
            {
                UserId = Guid.NewGuid(),
                UserName = "B",
                Phone = "2",
                Email = "b@email.com"
            }
        };

        _userRepositoryMock
            .Setup(r => r.GetUsers())
            .Returns(repoUsers);

        // Act
        var result = _userService.GetUsers();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));

        Assert.That(result[0].Id, Is.EqualTo(repoUsers[0].UserId));
        Assert.That(result[0].UserName, Is.EqualTo(repoUsers[0].UserName));
        Assert.That(result[0].Phone, Is.EqualTo(repoUsers[0].Phone));
        Assert.That(result[0].Email, Is.EqualTo(repoUsers[0].Email));

        Assert.That(result[1].Id, Is.EqualTo(repoUsers[1].UserId));
        Assert.That(result[1].UserName, Is.EqualTo(repoUsers[1].UserName));
        Assert.That(result[1].Phone, Is.EqualTo(repoUsers[1].Phone));
        Assert.That(result[1].Email, Is.EqualTo(repoUsers[1].Email));

        _userRepositoryMock.Verify(r => r.GetUsers(), Times.Once);
    }

    [Test]
    public void GetUsers_ShouldReturnEmptyLists_WhenNoUsersExist()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.GetUsers()).Returns(new List<TbUser>());

        // Act
        var result = _userService.GetUsers();

        // Assert
        Assert.That(result.Count, Is.EqualTo(0));
    }

    // -------------------------
    // UpdateUser
    // -------------------------

    [Test]
    public void UpdateUser_ShouldReturnMessage_WhenUserNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new UpdateUserRequest { Name = "Novo Nome" };

        _userRepositoryMock
            .Setup(r => r.GetById(id))
            .Returns((TbUser?)null);

        // Act
        var result = _userService.UpdateUser(id, request);

        // Assert
        Assert.That(result, Is.EqualTo($"Usuário com a id {id} não encontrado."));
        _userRepositoryMock.Verify(r => r.GetById(id), Times.Once);
        _userRepositoryMock.Verify(r => r.UpdateUser(It.IsAny<TbUser>()), Times.Never);
    }

    [Test]
    public void UpdateUser_ShouldUpdateAllProvidedFields_AndCallRepositoryUpdate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var user = new TbUser
        {
            UserId = id,
            UserName = "Antigo",
            Phone = "111",
            Email = "old@email.com"
        };

        var request = new UpdateUserRequest
        {
            Name = "Novo Nome",
            Phone = "222",
            Email = "new@email.com"
        };

        _userRepositoryMock
            .Setup(r => r.GetById(id))
            .Returns(user);

        _userRepositoryMock
            .Setup(r => r.UpdateUser(It.Is<TbUser>(u =>
                u.UserId == id &&
                u.UserName == "Novo Nome" &&
                u.Phone == "222" &&
                u.Email == "new@email.com"
            )));

        // Act
        var result = _userService.UpdateUser(id, request);

        // Assert
        Assert.That(result, Is.EqualTo(""));
        _userRepositoryMock.Verify(r => r.GetById(id), Times.Once);
        _userRepositoryMock.Verify(r => r.UpdateUser(It.IsAny<TbUser>()), Times.Once);
    }

    [Test]
    public void UpdateUser_ShouldOnlyUpdateNonNullFields()
    {
        // Arrange
        var id = Guid.NewGuid();
        var user = new TbUser
        {
            UserId = id,
            UserName = "Nome Atual",
            Phone = "999",
            Email = "atual@email.com"
        };

        var request = new UpdateUserRequest
        {
            Name = null,
            Phone = "123",
            Email = null
        };

        _userRepositoryMock
            .Setup(r => r.GetById(id))
            .Returns(user);

        _userRepositoryMock
            .Setup(r => r.UpdateUser(It.Is<TbUser>(u =>
                u.UserId == id &&
                u.UserName == "Nome Atual" &&     // não muda
                u.Phone == "123" &&               // muda
                u.Email == "atual@email.com"      // não muda
            )));

        // Act
        var result = _userService.UpdateUser(id, request);

        // Assert
        Assert.That(result, Is.EqualTo(""));
        _userRepositoryMock.Verify(r => r.GetById(id), Times.Once);
        _userRepositoryMock.Verify(r => r.UpdateUser(It.IsAny<TbUser>()), Times.Once);
    }

    // -------------------------
    // CreateUser
    // -------------------------

    [Test]
    public void CreateUser_ShouldReturnNull_WhenUserIsNull()
    {
        // Act
        var result = _userService.CreateUser(null!);

        // Assert
        Assert.That(result, Is.Null);
        _userRepositoryMock.Verify(r => r.CreateUser(It.IsAny<UserDTO>()), Times.Never);
    }

    [Test]
    public void CreateUser_ShouldCallRepositoryAndReturnSameUser_WhenUserIsValid()
    {
        // Arrange
        var user = new UserDTO
        {
            Id = Guid.NewGuid(),
            UserName = "Marco",
            Phone = "61999999999",
            Email = "marco@email.com"
        };

        _userRepositoryMock
            .Setup(r => r.CreateUser(user));

        // Act
        var result = _userService.CreateUser(user);

        // Assert
        Assert.That(result, Is.SameAs(user));
        _userRepositoryMock.Verify(r => r.CreateUser(user), Times.Once);
    }

    // -------------------------
    // GetUserOrders
    // -------------------------

    [Test]
    public void GetUserOrders_ShouldReturnNull_WhenUserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userRepositoryMock
            .Setup(r => r.GetById(userId))
            .Returns((TbUser?)null);

        // Act
        var result = _userService.GetUserOrders(userId);

        // Assert
        Assert.That(result, Is.Null);
        _userRepositoryMock.Verify(r => r.GetById(userId), Times.Once);
        _userRepositoryMock.Verify(r => r.GetUserOrders(It.IsAny<Guid>()), Times.Never);
    }

    [Test]
    public void GetUserOrders_ShouldReturnOrders_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new TbUser
        {
            UserId = userId,
            UserName = "User",
            Phone = "111",
            Email = "user@email.com"
        };

        var orders = new List<UserOrders>
        {
            new UserOrders
            {
                OrderId = Guid.NewGuid(),
                UserId = userId,
                UserName = "User",
                OrderStatus = OrderStatus.CONCLUDED
            }
        };

        _userRepositoryMock.Setup(r => r.GetById(userId)).Returns(user);
        _userRepositoryMock.Setup(r => r.GetUserOrders(userId)).Returns(orders);

        // Act
        var result = _userService.GetUserOrders(userId);

        // Assert
        Assert.That(result, Is.SameAs(orders));
        _userRepositoryMock.Verify(r => r.GetById(userId), Times.Once);
        _userRepositoryMock.Verify(r => r.GetUserOrders(userId), Times.Once);
    }    

}