using ContactListService.Controllers;
using ContactListService.Models;
using ContactListService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ContactListService.Tests;

public class ContactsControllerTests
{
    private readonly Mock<IContactService> _mockService;
    private readonly Mock<ILogger<ContactsController>> _mockLogger;
    private readonly ContactsController _controller;

    public ContactsControllerTests()
    {
        _mockService = new Mock<IContactService>();
        _mockLogger = new Mock<ILogger<ContactsController>>();
        _controller = new ContactsController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetContacts_ReturnsOkResult_WithPaginatedContacts()
    {
        // Arrange
        var contacts = new List<Contact> { new Contact(), new Contact() };
        var paginatedList = new PaginatedList<Contact>(contacts, 2, 1, 10);
        _mockService.Setup(service => service.GetContactsAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(paginatedList);

        // Act
        var result = await _controller.GetContacts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedContacts = Assert.IsType<PaginatedList<Contact>>(okResult.Value);
        Assert.Equal(2, returnedContacts.Items.Count);
    }

    [Fact]
    public async Task GetContact_ReturnsOkResult_WhenContactExists()
    {
        // Arrange
        var contact = new Contact { Id = 1, FirstName = "John", LastName = "Doe" };
        _mockService.Setup(service => service.GetContactByIdAsync(1))
            .ReturnsAsync(contact);

        // Act
        var result = await _controller.GetContact(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedContact = Assert.IsType<Contact>(okResult.Value);
        Assert.Equal(1, returnedContact.Id);
        Assert.Equal("John", returnedContact.FirstName);
    }

    [Fact]
    public async Task CreateContact_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("FirstName", "Required");

        // Act
        var result = await _controller.CreateContact(new Contact());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateContact_ReturnsCreatedAtAction_WhenContactIsValid()
    {
        // Arrange
        var newContact = new Contact
            { FirstName = "Jane", LastName = "Doe", PhoneNumber = "1234567890", Email = "jane@example.com" };
        var createdContact = new Contact
            { Id = 1, FirstName = "Jane", LastName = "Doe", PhoneNumber = "1234567890", Email = "jane@example.com" };
        _mockService.Setup(service => service.AddContactAsync(newContact))
            .ReturnsAsync(createdContact);

        // Act
        var result = await _controller.CreateContact(newContact);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal("GetContact", createdAtActionResult.ActionName);
        Assert.Equal(1, createdAtActionResult.RouteValues["id"]);
    }

    [Fact]
    public async Task UpdateContact_ReturnsNotFound_WhenContactDoesNotExist()
    {
        // Arrange
        var contact = new Contact
            { Id = 1, FirstName = "Jane", LastName = "Doe", PhoneNumber = "1234567890", Email = "jane@example.com" };
        _mockService.Setup(service => service.UpdateContactAsync(1, contact))
            .ReturnsAsync((Contact)null);

        // Act
        var result = await _controller.UpdateContact(1, contact);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteContact_ReturnsNotFound_WhenContactDoesNotExist()
    {
        // Arrange
        _mockService.Setup(service => service.DeleteContactAsync(1))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteContact(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteContact_ReturnsNoContent_WhenContactIsDeleted()
    {
        // Arrange
        _mockService.Setup(service => service.DeleteContactAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteContact(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}