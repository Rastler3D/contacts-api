using ContactListService.Models;
using ContactListService.Repositories;

namespace ContactListService.Services;

/// <summary>
/// Service for managing contacts
/// </summary>
public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;

    /// <summary>
    /// Initializes a new instance of the ContactService
    /// </summary>
    /// <param name="contactRepository">The contact repository</param>
    public ContactService(IContactRepository contactRepository)
    {
        _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
    }

    /// <inheritdoc/>
    public async Task<PaginatedList<Contact>> GetContactsAsync(int pageNumber, int pageSize)
    {
        return await _contactRepository.GetContactsAsync(pageNumber, pageSize);
    }

    /// <inheritdoc/>
    public async Task<Contact?> GetContactByIdAsync(int id)
    {
        return await _contactRepository.GetContactByIdAsync(id);
    }

    /// <inheritdoc/>
    public async Task<Contact> AddContactAsync(Contact contact)
    {
        return await _contactRepository.CreateContactAsync(contact);
    }

    /// <inheritdoc/>
    public async Task<Contact?> UpdateContactAsync(int id, Contact contact)
    {
        return await _contactRepository.UpdateContactAsync(id, contact);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteContactAsync(int id)
    {
        return await _contactRepository.DeleteContactAsync(id);
    }
}