using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
}