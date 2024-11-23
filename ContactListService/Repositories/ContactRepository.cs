using ContactListService.Data;
using ContactListService.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactListService.Repositories;

/// <summary>
/// Contact repository for data access operations
/// </summary>
public class ContactRepository : IContactRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the ContactRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public ContactRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public async Task<PaginatedList<Contact>> GetContactsAsync(int pageNumber, int pageSize)
    {
        var query = _context.Contacts
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName);

        return await PaginatedList<Contact>.CreateAsync(query, pageNumber, pageSize);
    }

    /// <inheritdoc/>
    public async Task<Contact?> GetContactByIdAsync(int id)
    {
        return await _context.Contacts
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <inheritdoc/>
    public async Task<Contact> CreateContactAsync(Contact contact)
    {
        contact.CreatedAt = DateTime.UtcNow;
        await _context.Contacts.AddAsync(contact);
        await _context.SaveChangesAsync();

        return contact;
    }

    /// <inheritdoc/>
    public async Task<Contact?> UpdateContactAsync(int id, Contact contact)
    {
        var existingContact = await _context.Contacts.FindAsync(id);
        if (existingContact == null)
        {
            return null;
        }

        existingContact.FirstName = contact.FirstName;
        existingContact.LastName = contact.LastName;
        existingContact.PhoneNumber = contact.PhoneNumber;
        existingContact.Email = contact.Email;
        existingContact.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existingContact;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteContactAsync(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);
        if (contact == null)
        {
            return false;
        }

        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();
        return true;
    }
}