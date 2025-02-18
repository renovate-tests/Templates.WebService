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
    public class ContactsApiFacts : ApiFactsBase
    {
        public ContactsApiFacts(ITestOutputHelper output)
            : base(output)
        {
            AsUser("user1");
        }

        private readonly Mock<IContactService> _serviceMock = new Mock<IContactService>();

        protected override void ConfigureService(IServiceCollection services)
            => services.AddMock(_serviceMock);

        [Fact]
        public async Task ReadsAllFromService()
        {
            var contacts = new List<Contact>
            {
                new Contact {Id = "1", FirstName = "John", LastName = "Smith"},
                new Contact {Id = "2", FirstName = "Jane", LastName = "Doe"}
            };
            _serviceMock.Setup(x => x.ReadAllAsync()).ReturnsAsync(contacts);

            var result = await Client.Contacts.ReadAllAsync();

            result.Should().Equal(contacts);
        }

        [Fact]
        public async Task RejectsUnauthenticatedRead()
        {
            AsAnonymous();
            await Client.Contacts.Awaiting(x => x.ReadAllAsync())
                        .Should().ThrowAsync<AuthenticationException>();
        }

        [Fact]
        public async Task ReadsFromService()
        {
            var contact = new Contact {Id = "1", FirstName = "John", LastName = "Smith"};
            _serviceMock.Setup(x => x.ReadAsync("1")).ReturnsAsync(contact);

            var result = await Client.Contacts["1"].ReadAsync();

            result.Should().Be(contact);
        }

        [Fact]
        public async Task CreatesInService()
        {
            var contactWithoutId = new Contact {FirstName = "John", LastName = "Smith"};
            var contactWithId = new Contact {Id = "1", FirstName = "John", LastName = "Smith"};
            _serviceMock.Setup(x => x.CreateAsync(contactWithoutId)).ReturnsAsync(contactWithId);

            var result = await Client.Contacts.CreateAsync(contactWithoutId);

            result.Uri.Should().Be("http://localhost/contacts/1/");
        }

        [Fact]
        public async Task RejectsCreateOnIncompleteBody()
        {
            await Client.Contacts.Awaiting(x => x.CreateAsync(new Contact()).AsTask())
                        .Should().ThrowAsync<InvalidDataException>();
        }

        [Fact]
        public async Task UpdatesInService()
        {
            var contact = new Contact {Id = "1", FirstName = "John", LastName = "Smith"};

            await Client.Contacts.SetAsync(contact);

            _serviceMock.Verify(x => x.UpdateAsync(contact));
        }

        [Fact]
        public async Task RejectsUpdateOnIdMismatch()
        {
            var contactDto = new Contact {Id = "1", FirstName = "John", LastName = "Smith"};

            await Client.Contacts["2"].Awaiting(x => x.SetAsync(contactDto))
                        .Should().ThrowAsync<InvalidDataException>();
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
            var note = new Note {Content = "my note"};
            _serviceMock.Setup(x => x.ReadNoteAsync("1")).ReturnsAsync(note);

            AsUser("user1", Scopes.Notes);
            var result = await Client.Contacts["1"].Note.ReadAsync();

            result.Should().Be(note);
        }

        [Fact]
        public async Task SetsNoteInService()
        {
            var note = new Note {Content = "my note"};

            AsUser("user1", Scopes.Notes);
            await Client.Contacts["1"].Note.SetAsync(note);

            _serviceMock.Verify(x => x.SetNoteAsync("1", note));
        }

        [Fact]
        public async Task PokesViaService()
        {
            AsUser("user1", Scopes.Poke);
            await Client.Contacts["1"].Poke.TriggerAsync();

            _serviceMock.Verify(x => x.PokeAsync("1"));
        }

        [Fact]
        public async Task RejectsUnauthorizedPoke()
        {
            await Client.Contacts["1"].Poke.Awaiting(x => x.TriggerAsync())
                        .Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task RejectsUnauthenticatedPoke()
        {
            AsAnonymous();
            await Client.Contacts["1"].Poke.Awaiting(x => x.TriggerAsync())
                        .Should().ThrowAsync<AuthenticationException>();
        }
    }
}
