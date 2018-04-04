using System.Linq;
using System.Threading.Tasks;
using Axoom.MyService.Database;
using Axoom.MyService.Dto;
using FluentAssertions;
using Xunit;

namespace Axoom.MyService.Services
{
    public class ContactServiceFacts : DatabaseFactsBase
    {
        private readonly IContactService _service;

        public ContactServiceFacts() => _service = new ContactService(Context);

        [Fact]
        public async Task ReadsAllFromDatabase()
        {
            string id1 = Context.Contacts.Add(new ContactEntity {FirstName = "John", LastName = "Smith"}).Entity.Id;
            string id2 = Context.Contacts.Add(new ContactEntity {FirstName = "Jane", LastName = "Doe"}).Entity.Id;
            Context.SaveChanges();

            var result = await _service.ReadAllAsync();
            result.Should().Equal(
                new ContactDto {Id = id1, FirstName = "John", LastName = "Smith"},
                new ContactDto {Id = id2, FirstName = "Jane", LastName = "Doe"});
        }

        [Fact]
        public async Task ReadsFromDatabase()
        {
            string id = Context.Contacts.Add(new ContactEntity {FirstName = "John", LastName = "Smith"}).Entity.Id;
            Context.SaveChanges();

            var result = await _service.ReadAsync(id);
            result.Should().Be(new ContactDto {Id = id, FirstName = "John", LastName = "Smith"});
        }

        [Fact]
        public async Task CreatesInDatabase()
        {
            var result = await _service.CreateAsync(new ContactDto {FirstName = "John", LastName = "Smith"});
            Context.Contacts.Single().Should().BeEquivalentTo(new ContactEntity {Id = result.Id, FirstName = "John", LastName = "Smith"});
        }

        [Fact]
        public async Task UpdatesInDatabase()
        {
            string id = Context.Contacts.Add(new ContactEntity {FirstName = "John", LastName = "Smith"}).Entity.Id;
            Context.SaveChanges();

            await _service.UpdateAsync(new ContactDto {Id = id, FirstName = "Jane", LastName = "Doe"});

            var entity = Context.Contacts.Find(id);
            entity.FirstName.Should().Be("Jane");
            entity.LastName.Should().Be("Doe");
        }

        [Fact]
        public async Task DeletesFromDatabase()
        {
            string id = Context.Contacts.Add(new ContactEntity()).Entity.Id;
            Context.SaveChanges();

            await _service.DeleteAsync(id);

            Context.Contacts.Should().BeEmpty();
        }

        [Fact]
        public async Task ReadsNoteFromDatabase()
        {
            string id = Context.Contacts.Add(new ContactEntity {FirstName = "John", LastName = "Smith", Note = "my note"}).Entity.Id;
            Context.SaveChanges();

            var note = await _service.ReadNoteAsync(id);
            note.Should().Be(new NoteDto {Content = "my note"});
        }

        [Fact]
        public async Task WritesNoteInDatabase()
        {
            string id = Context.Contacts.Add(new ContactEntity {FirstName = "John", LastName = "Smith"}).Entity.Id;
            Context.SaveChanges();

            await _service.SetNoteAsync(id, new NoteDto{Content = "my note"});

            Context.Contacts.Find(id).Note.Should().Be("my note");
        }

        [Fact]
        public async Task StoresPokeInDatabase()
        {
            string id = Context.Contacts.Add(new ContactEntity {FirstName = "John", LastName = "Smith"}).Entity.Id;
            Context.SaveChanges();

            await _service.PokeAsync(id);

            Context.Pokes.Single().ContactId.Should().Be(id);
        }
    }
}