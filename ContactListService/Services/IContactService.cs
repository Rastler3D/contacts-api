using ContactListService.Models;

namespace ContactListService.Services;

/// <summary>
/// Service interface for contact operations
/// </summary>
public interface IContactService
{
    /// <summary>
    /// Retrieves a paginated list of contacts
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve</param>
    /// <param name="pageSize">The number of items per page</param>
    /// <returns>A paginated list of contacts</returns>
    Task<PaginatedList<Contact>> GetContactsAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Retrieves a specific contact by its ID
    /// </summary>
    /// <param name="id">The ID of the contact to retrieve</param>
    /// <returns>The requested contact, or null if not found</returns>
    Task<Contact?> GetContactByIdAsync(int id);

    /// <summary>
    /// Adds a new contact
    /// </summary>
    /// <param name="contact">The contact to add</param>
    /// <returns>The newly created contact</returns>
    Task<Contact> AddContactAsync(Contact contact);

    /// <summary>
    /// Updates an existing contact
    /// </summary>
    /// <param name="id">The ID of the contact to update</param>
    /// <param name="contact">The updated contact information</param>
    /// <returns>The updated contact, or null if not found</returns>
    Task<Contact?> UpdateContactAsync(int id, Contact contact);

    /// <summary>
    /// Deletes a contact
    /// </summary>
    /// <param name="id">The ID of the contact to delete</param>
    /// <returns>True if the contact was deleted, false if not found</returns>
    Task<bool> DeleteContactAsync(int id);
}