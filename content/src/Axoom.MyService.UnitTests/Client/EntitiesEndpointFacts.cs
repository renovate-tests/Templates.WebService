using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Axoom.MyService.Dto;
using Axoom.MyService.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TypedRest;
using Xunit;

namespace Axoom.MyService.Client
{
    public class EntitiesEndpointFacts : EndpointFactsBase
    {
        private readonly Mock<IEntityService> _serviceMock = new Mock<IEntityService>();

        protected override void ConfigureService(IServiceCollection services)
            => services.AddMock(_serviceMock);

        [Fact]
        public async Task GetsAllFromService()
        {
            var entities = new List<Entity>
            {
                new Entity {Id = "1", Data = "a"},
                new Entity {Id = "2", Data = "b"}
            };
            _serviceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(entities);

            var result = await Client.Entities.ReadAllAsync();

            result.Should().Equal(entities);
        }

        [Fact]
        public async Task GetsFromService()
        {
            var entity = new Entity {Id = "1", Data = "a"};
            _serviceMock.Setup(x => x.GetAsync("1")).ReturnsAsync(entity);

            var result = await Client.Entities["1"].ReadAsync();

            result.Should().Be(entity);
        }

        [Fact]
        public async Task AddsToService()
        {
            var entityWithoutId = new Entity {Data = "a"};
            var entityWithId = new Entity {Id = "1", Data = "a"};
            _serviceMock.Setup(x => x.AddAsync(entityWithoutId)).ReturnsAsync(entityWithId);

            var result = await Client.Entities.CreateAsync(entityWithoutId);

            result.Uri.Should().Be("http://localhost/entities/1");
        }

        [Fact]
        public async Task ModifiesInService()
        {
            var entity = new Entity {Id = "1", Data = "a"};
            _serviceMock.Setup(x => x.ModifyAsync(entity)).Returns(Task.CompletedTask);

            await Client.Entities.SetAsync(entity);
        }

        [Fact]
        public void RejectsUpdateOnIdMismatch()
        {
            Func<Task> setAsync = async () =>
            {
                await Client.Entities["2"].SetAsync(new Entity {Id = "1", Data = "a"});
            };
            setAsync.Should().Throw<InvalidDataException>();
        }

        [Fact]
        public async Task RemovesFromService()
        {
            _serviceMock.Setup(x => x.RemoveAsync("1")).Returns(Task.CompletedTask);

            await Client.Entities["1"].DeleteAsync();
        }
    }
}