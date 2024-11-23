using System.ComponentModel.DataAnnotations;
using ContactListService.Models;
using ContactListService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactListService.Controllers;

/// <summary>
/// Controller for managing contacts
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly ILogger<ContactsController> _logger;

    /// <summary>
    /// Initializes a new instance of the ContactsController
    /// </summary>
    /// <param name="contactService">The contact service</param>
    /// <param name="logger">The logger</param>
    public ContactsController(IContactService contactService, ILogger<ContactsController> logger)
    {
        _contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Get paginated list of contacts
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<Contact>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaginatedList<Contact>>> GetContacts([FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            _logger.LogInformation("Getting contacts: page {PageNumber}, pageSize {PageSize}", pageNumber, pageSize);
            var paginatedContacts = await _contactService.GetContactsAsync(pageNumber, pageSize);
            return Ok(paginatedContacts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting contacts");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    /// <summary>
    /// Get contact by ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Contact), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Contact>> GetContact(int id)
    {
        try
        {
            _logger.LogInformation("Getting contact by id: {Id}", id);
            var contact = await _contactService.GetContactByIdAsync(id);

            if (contact == null)
            {
                _logger.LogWarning("Contact not found: {Id}", id);
                return NotFound();
            }

            return Ok(contact);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting contact with id: {Id}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    /// <summary>
    /// Create new contact
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Contact), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Contact>> CreateContact(Contact contact)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for create contact: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new contact: {@Contact}", contact);
            var createdContact = await _contactService.AddContactAsync(contact);
            return CreatedAtAction(nameof(GetContact), new { id = createdContact.Id }, createdContact);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating contact: {@Contact}", contact);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    /// <summary>
    /// Update existing contact
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateContact(int id, Contact contact)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for update contact: {@ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating contact: {Id}, {@Contact}", id, contact);
            var updatedContact = await _contactService.UpdateContactAsync(id, contact);

            if (updatedContact == null)
            {
                _logger.LogWarning("Contact not found for update: {Id}", id);
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating contact: {Id}, {@Contact}", id, contact);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    /// <summary>
    /// Delete contact
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteContact(int id)
    {
        try
        {
            _logger.LogInformation("Deleting contact: {Id}", id);
            var result = await _contactService.DeleteContactAsync(id);

            if (!result)
            {
                _logger.LogWarning("Contact not found for deletion: {Id}", id);
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting contact: {Id}", id);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}