using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Axoom.MyService.Contacts
{
    /// <summary>
    /// Manages contacts in an address book.
    /// </summary>
    public class ContactService : IContactService
    {
        private readonly MyServiceDbContext _context;
        private readonly IContactMetrics _metrics;

        public ContactService(MyServiceDbContext context, IContactMetrics metrics)
        {
            _context = context;
            _metrics = metrics;
        }

        public async Task<IEnumerable<ContactDto>> ReadAllAsync()
            => await ToDtos(_context.Contacts).ToListAsync();

        public async Task<ContactDto> ReadAsync(string id)
        {
            var element = await ToDtos(_context.Contacts.Where(x => x.Id == id)).SingleOrDefaultAsync();
            if (element == null) throw new KeyNotFoundException($"Contact '{id}' not found.");

            return element;
        }

        private static IQueryable<ContactDto> ToDtos(IQueryable<ContactEntity> entities)
            => entities.Select(x => new ContactDto {Id = x.Id, FirstName = x.FirstName, LastName = x.LastName});

        public async Task<ContactDto> CreateAsync(ContactDto element)
        {
            var entity = new ContactEntity();
            FromDtoToEntity(element, entity);

            using (_metrics.TimerWrite())
            {
                await _context.Contacts.AddAsync(entity);
                await _context.SaveChangesAsync();
                return new ContactDto {Id = entity.Id, FirstName = element.FirstName, LastName = element.LastName};
            }
        }

        public async Task UpdateAsync(ContactDto element)
        {
            var entity = await _context.Contacts.FindAsync(element.Id);
            if (entity == null) throw new KeyNotFoundException($"Contact '{element.Id}' not found.");

            FromDtoToEntity(element, entity);

            using (_metrics.TimerWrite())
            {
                _context.Update(entity);
                await _context.SaveChangesAsync();
            }
        }

        private static void FromDtoToEntity(ContactDto dto, ContactEntity entity)
        {
            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await _context.Contacts.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException($"Contact '{id}' not found.");

            _context.Contacts.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<NoteDto> ReadNoteAsync(string id)
        {
            var note = await _context.Contacts.Where(x => x.Id == id).Select(x => new NoteDto {Content = x.Note}).SingleOrDefaultAsync();
            if (note == null) throw new KeyNotFoundException($"Contact '{id}' not found.");

            return note;
        }

        public async Task SetNoteAsync(string id, NoteDto note)
        {
            var entity = await _context.Contacts.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException($"Contact '{id}' not found.");

            entity.Note = note.Content;

            using (_metrics.TimerWrite())
            {
                _context.Update(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task PokeAsync(string id)
        {
            var entity = await _context.Contacts.FindAsync(id);
            if (entity == null) throw new KeyNotFoundException($"Contact '{id}' not found.");

            _metrics.Poke();
            
            entity.Pokes.Add(new PokeEntity {Timestamp = DateTime.UtcNow});
            await _context.SaveChangesAsync();
        }
    }
}
