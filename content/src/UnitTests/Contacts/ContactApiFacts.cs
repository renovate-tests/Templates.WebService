using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MorseCode.ITask;
using TypedRest;
using Xunit;
using Xunit.Abstractions;

namespace MyVendor.MyService.Contacts
{
    public class ContactApiFacts : ApiFactsBase
    {
        public ContactApiFacts(ITestOutputHelper output)
            : base(output)
        {}

        private readonly Mock<IContactService> _serviceMock = new Mock<IContactService>();

        protected override void ConfigureService(IServiceCollection services)
            => services.AddMock(_serviceMock);

        [Fact]
        public async Task ReadsAllFromService()
        {
            var contacts = new List<ContactDto>
            {
                new ContactDto {Id = "1", FirstName = "John", LastName = "Smith"},
                new ContactDto {Id = "2", FirstName = "Jane", LastName = "Doe"}
            };
            _serviceMock.Setup(x => x.ReadAllAsync()).ReturnsAsync(contacts);

            var result = await Client.Contacts.ReadAllAsync();

            result.Should().Equal(contacts);
        }

        [Fact]
        public async Task ReadsFromService()
        {
            var contact = new ContactDto {Id = "1", FirstName = "John", LastName = "Smith"};
            _serviceMock.Setup(x => x.ReadAsync("1")).ReturnsAsync(contact);

            var result = await Client.Contacts["1"].ReadAsync();

            result.Should().Be(contact);
        }

        [Fact]
        public async Task CreatesInService()
        {
            var contactWithoutId = new ContactDto {FirstName = "John", LastName = "Smith"};
            var contactWithId = new ContactDto {Id = "1", FirstName = "John", LastName = "Smith"};
            _serviceMock.Setup(x => x.CreateAsync(contactWithoutId)).ReturnsAsync(contactWithId);

            var result = await Client.Contacts.CreateAsync(contactWithoutId);

            result.Uri.Should().Be("http://localhost/contacts/1/");
        }

        [Fact]
        public void RejectsCreateOnIncompleteBody()
        {
            Client.Contacts.Awaiting(x => x.CreateAsync(new ContactDto()).AsTask()).Should().Throw<InvalidDataException>();
        }

        [Fact]
        public async Task UpdatesInService()
        {
            var contact = new ContactDto {Id = "1", FirstName = "John", LastName = "Smith"};

            await Client.Contacts.SetAsync(contact);

            _serviceMock.Verify(x => x.UpdateAsync(contact));
        }

        [Fact]
        public void RejectsUpdateOnIdMismatch()
        {
            var contactDto = new ContactDto {Id = "1", FirstName = "John", LastName = "Smith"};

            Client.Contacts["2"].Awaiting(x => x.SetAsync(contactDto))
                  .Should().Throw<InvalidDataException>();
        }

        [Fact]
        public async Task DeletesFromService()
        {
            await Client.Contacts["1"].DeleteAsync();

            _serviceMock.Verify(x => x.DeleteAsync("1"));
        }

        [Fact]
        public async Task ReadsNoteFromService()
        {
            var note = new NoteDto {Content = "my note"};
            _serviceMock.Setup(x => x.ReadNoteAsync("1")).ReturnsAsync(note);

            var result = await Client.Contacts["1"].Note.ReadAsync();

            result.Should().Be(note);
        }

        [Fact]
        public async Task SetsNoteInService()
        {
            var note = new NoteDto {Content = "my note"};

            await Client.Contacts["1"].Note.SetAsync(note);

            _serviceMock.Verify(x => x.SetNoteAsync("1", note));
        }

        [Fact]
        public async Task PokesViaService()
        {
            await Client.Contacts["1"].Poke.TriggerAsync();

            _serviceMock.Verify(x => x.PokeAsync("1"));
        }
    }
}
