using ContactListService.Models;

namespace ContactListService.Repositories;

/// <summary>
/// Repository interface for contact data access operations
/// </summary>
public interface IContactRepository
{
    /// <summary>
    /// Retrieves a paginated list of contacts
    /// </summary>
    /// <param name="pageNumber">The current page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <returns>Tuple containing the list of contacts and total count</returns>
    Task<PaginatedList<Contact>> GetContactsAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Retrieves a specific contact by ID
    /// </summary>
    /// <param name="id">The contact ID</param>
    /// <returns>Contact if found, null otherwise</returns>
    Task<Contact?> GetContactByIdAsync(int id);

    /// <summary>
    /// Creates a new contact in the database
    /// </summary>
    /// <param name="contact">The contact to create</param>
    /// <returns>The created contact with generated ID</returns>
    Task<Contact> CreateContactAsync(Contact contact);

    /// <summary>
    /// Updates an existing contact in the database
    /// </summary>
    /// <param name="id">The id of the contact to update</param>
    /// <param name="contact">The contact to update</param>
    /// <returns>The updated contact</returns>
    Task<Contact?> UpdateContactAsync(int id, Contact contact);

    /// <summary>
    /// Deletes a contact from the database
    /// </summary>
    /// <param name="id">The ID of the contact to delete</param>
    /// <returns>True if contact was deleted, false otherwise</returns>
    Task<bool> DeleteContactAsync(int id);
}